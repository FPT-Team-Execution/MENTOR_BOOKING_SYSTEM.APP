using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MBS.BusinessObject.Entities;
using MBS.BusinessObject.Enums;
using MBS.Externals.Models.Email;
using MBS.Externals.Models.Google.GoogleOAuth.Response;
using MBS.Externals.Services.Interfaces;
using MBS.Externals.Templates;
using MBS.Razor.Pages.AdminPage;
using MBS.Services.Constants;
using MBS.Services.Constants.Enums;
using MBS.Services.Models;
using MBS.Services.Models.Requests.Auth;
using MBS.Services.Models.Sessions;
using MBS.Services.Services.Interfaces;
using MBS.Services.Shared;
using MBS.Services.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace MBS.Razor.Pages
{
    public class LoginModel : BaseAdminPage
    {
        private readonly IClaimService _claimService;
        private IAuthService _authService;
        private readonly IGoogleService _googleService;
        private IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        public LoginModel(IClaimService claimService, IAuthService authService, IConfiguration configuration, IGoogleService googleService, UserManager<ApplicationUser> userManager)
        {
            this._authService = authService;
            _configuration = configuration;
            _claimService = claimService;
            _googleService = googleService;
            _userManager = userManager;
        }

        [BindProperty] public LoginRequest LoginRequest { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _authService.GetUserByEmailAsync(LoginRequest.Email);

            if (user is null)
            {
                TempData["ErrorMessage"] = "Email or password incorrect!";
                return Page();
            }

            var isPasswordCorrect = await _authService.IsPasswordCorrect(user, LoginRequest.Password);

            if (!isPasswordCorrect)
            {
                TempData["ErrorMessage"] = "Email or password incorrect!";
                return Page();
            }

            if (!user.EmailConfirmed)
            {
                await _authService.SendVerifyEmail(user);
                TempData["ErrorMessage"] = "You need to confirm email!";
                SaveTempData("Email", user.Email!);
                return Redirect(RouteEndpoints.ConfirmEmail);
            }

            var userRole = await _authService.GetUserRoleAsync(user);

            var claims = new List<Claim>
            {
                //User Name
                new Claim(ClaimTypes.Name, user.Email!),
                //Role
                new Claim(ClaimTypes.Role, userRole),
                //User Id
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            //save it to cookie
            await _claimService.SignInAsync(claims);
            //append access token
            _claimService.AppendCookie(CookieNames.UserId, user.Id);
            _claimService.AppendCookie(CookieNames.UserEmail, user.Email);
            _claimService.AppendCookie(CookieNames.UserRole, userRole);
            //var claims = GetClaims(response.ResponseModel.JwtToken.AccessToken);
            //await _claimService.SignInAsync(claims);
            TempData["SuccessMessage"] = "Login successfully";

            return claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)!.Value.ToString() switch
            {
                UserRole.Admin => Redirect(RouteEndpoints.AdminDashboard),
                UserRole.Student => Redirect(RouteEndpoints.StudentProject),
                UserRole.Mentor => Redirect(RouteEndpoints.MentorMeeting),
                _ => Redirect(RouteEndpoints.Login)
            };
        }

        /// <summary>
        /// Redirect to google sign in
        /// </summary>
        public void OnGetLoginWithGoogle()
        {
            var googleRedirectUrl = _authService.GetGoogleRedirectUrl();
            Response.Redirect(googleRedirectUrl);
        }

        /// <summary>
        /// Google Callback uri by name handler
        /// </summary>
        public async Task<IActionResult> OnGetCallback(string code, string state, string scopes)
        {
            try
            {
                //get return url from appsettings
                var googleAuthSettings = _configuration.GetSection("Google:Auth");
                var redirectUrl = googleAuthSettings["RedirectUrl"];

                //get auth token
                var tokenResponse = await _googleService.GetTokenGoogleUserAsync(code, redirectUrl!);
                if (!tokenResponse.IsSuccess)
                {
                    SaveTempDataString(TempDataKeys.ErrorMessage, "Login by Google failed!");
                    return Redirect(RouteEndpoints.Login);
                }

                var gtokenResponse = (GoogleTokenResponse)tokenResponse;
                var profileResponse =
                    await _googleService.GetProfileGoogleUserAsync(gtokenResponse.access_token);
                if (!profileResponse.IsSuccess)
                {
                    SaveTempDataString(TempDataKeys.ErrorMessage, "Login by Google failed!");
                    return Redirect(RouteEndpoints.Login);
                }

                //Check user is student or not 
                var profile = (GoogleUserInfoResponse)profileResponse;
                var studentCheck = await _userManager.FindByEmailAsync(profile.email);
                var studentRole = await _userManager.GetRolesAsync(studentCheck!);
                if (studentCheck != null && studentRole.Contains(UserRoleEnum.Student.ToString()))
                {
                    SaveTempDataString(TempDataKeys.ErrorMessage, "Only mentor allowed to login by Google");
                    return Redirect(RouteEndpoints.Login);
                }
                //SignUp Or Sign In 

                var user = await _authService.LoginWithGoogleAsync(profile);
                if (!profileResponse.IsSuccess)
                {
                    SaveTempDataString(TempDataKeys.ErrorMessage, "Login by Google failed!");
                    return Redirect(RouteEndpoints.Login);
                }

                var userRole = await _authService.GetUserRoleAsync(user);
                //Save access_token of google to cookie
                var token = (GoogleTokenResponse)tokenResponse;
                _claimService.AppendCookie(CookieNames.UserId, user.Id);
                _claimService.AppendCookie(CookieNames.UserEmail, user.Email);
                _claimService.AppendCookie(CookieNames.UserRole, userRole);
                _claimService.AppendCookie(CookieNames.GoogleAccessToken, token.access_token);

                return Redirect(RouteEndpoints.Mentor);
            }
            catch (Exception e)
            {
                SaveTempDataString(TempDataKeys.ErrorMessage, "Error. Failed");
                return Redirect(RouteEndpoints.Login);
            }
        }

        /// <summary>
        /// Logout
        /// </summary>
        public async void OnGetLogOut()
        {
            // HttpContext.Session.Clear();
            // Response.Redirect(RouteEndpoints.Login);
            await _claimService.SignOutAsync();
            Response.Redirect(RouteEndpoints.Login);
        }

        private List<Claim> GetClaims(string token)
        {
            // var tokenGoogle = response.ResponseRequestModel.googleToken;
            // Decode the token to extract claims
            var handler = new JwtSecurityTokenHandler();
            var jwtInfo = handler.ReadJwtToken(token);
            var claims = new List<Claim>
            {
                //User Name
                new Claim(ClaimTypes.Name,
                    jwtInfo.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)!.Value.ToString()),
                //Role
                new Claim(ClaimTypes.Role,
                    jwtInfo.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)!.Value.ToString()),
                //User Id
                new Claim(ClaimTypes.NameIdentifier,
                    jwtInfo.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value.ToString())
            };
            return claims;
        }
    }
}