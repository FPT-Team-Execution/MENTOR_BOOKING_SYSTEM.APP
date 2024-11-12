using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MBS.Services.Constants;
using MBS.Services.Constants.Enums;
using MBS.Services.Models.Requests.Auth;
using MBS.Services.Models.Sessions;
using MBS.Services.Services.Interfaces;
using MBS.Services.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace MBS.Razor.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IClaimService _claimService;
        private IAuthService _authService;
        private IConfiguration _configuration;

        public LoginModel(IClaimService claimService, IAuthService authService, IConfiguration configuration)
        {
            this._authService = authService;
            _configuration = configuration;
            _claimService = claimService;
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
                TempData["ErrorMessage"] = "Email or password incorrect!";
                return Page();
            }

            var claims = new List<Claim>
            {
                //User Name
                new Claim(ClaimTypes.Name, user.Email!),
                //Role
                new Claim(ClaimTypes.Role, await _authService.GetUserRoleAsync(user)),
                //User Id
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            //save it to cookie
            await _claimService.SignInAsync(claims);
            //append access token
            _claimService.AppendCookie("USER_ID", user.Id);
            _claimService.AppendCookie("USER_EMAIL", user.Email);
            _claimService.AppendCookie("USER_ROLE", user.Email);
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
            //take token
            var response = await _authService.LoginWithGoogleAsync(code);
            var accessToken = response.ResponseRequestModel.jwtModel.AccessToken;
            var claims = GetClaims(accessToken);
            //save it to cookie
            await _claimService.SignInAsync(claims);
            //append access token
            _claimService.AppendCookie("MBS", accessToken);
            return Redirect(RouteEndpoints.Mentor);
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