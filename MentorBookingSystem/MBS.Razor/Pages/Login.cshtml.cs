using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MBS.Services.Constants;
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

        public LoginModel(IClaimService claimService,IAuthService authService, IConfiguration configuration)
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

            var response = await _authService.LoginAsync(LoginRequest);

            if (!response.StatusCode.Equals(StatusCodes.Status200OK) || response.ResponseModel == null)
            {
                TempData["ErrorMessage"] = response.Message;
                return Page();
            }
            var claims = GetClaims(response.ResponseModel.JwtToken.AccessToken);
            await _claimService.SignInAsync(claims);
            TempData["SuccessMessage"] = response.Message;

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

            var claims = GetClaims(response.ResponseRequestModel.jwtModel.AccessToken);
            //save it to cookie
            await _claimService.SignInAsync(claims);
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
                new Claim(ClaimTypes.Name, jwtInfo.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)!.Value.ToString()),
                //Role
                new Claim(ClaimTypes.Role, jwtInfo.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)!.Value.ToString()),
                //User Id
                new Claim(ClaimTypes.Role, jwtInfo.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value.ToString())
            };
            return claims;
        }
    }
}