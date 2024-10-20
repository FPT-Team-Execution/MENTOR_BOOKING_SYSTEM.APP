using MBS.Services.Constants;
using MBS.Services.Models.Requests.Auth;
using MBS.Services.Services.Interfaces;
using MBS.Services.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MBS.Razor.Pages
{
    public class LoginModel : PageModel
    {
        private IAuthService _authService;

        public LoginModel(IAuthService authService)
        {
            this._authService = authService;
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

            if (!response.StatusCode.Equals(StatusCodes.Status200OK))
            {
                TempData["ErrorMessage"] = response.Message;
                return Page();
            }

            HttpContext.Session.SetString("AccessToken", response.ResponseModel.JwtToken.AccessToken);
            HttpContext.Session.SetString("RefreshToken", response.ResponseModel.JwtToken.RefreshToken);
            
            TempData["SuccessMessage"] = response.Message;
            return Redirect(RouteEndpoints.AdminDashboard);
        }
        /// <summary>
        /// Redirect to google sign in
        /// </summary>
        public void OnGetLoginWithGoogle()
        {
            var clientId = "1096581745243-bj51g3cd4rq13codonsoftbk8h7tq4mi.apps.googleusercontent.com";
            var redirectUrl = "https://localhost:7102/AuthCallBack";
            //better solution
            // var redirectUrl = $"http://localhost:7102/Login?handler={RouteEndpointHandlers.Callback}";

            var state = Guid.NewGuid().ToString();
            var scope = "openid email profile";
            var responseType = "code";
            var googleAuthUrl = $"https://accounts.google.com/o/oauth2/v2/auth?redirect_uri={redirectUrl}&prompt=consent&response_type={responseType}&" +
                                $"client_id={clientId}&scope={scope}&access_type=offline&state={state}";
            Response.Redirect(googleAuthUrl);
        }
        /// <summary>
        /// Google Callback uri by name handler
        /// </summary>
        public void OnGetCallback(string code, string state, string scopes)
        {
            Response.Redirect("/Login");
        }
    }
}