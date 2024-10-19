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
        
        [BindProperty]
        public LoginRequest LoginRequest { get; set; }
        
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
            HttpContext.Session.SetString("AccessToken", response.ResponseModel.JwtToken.AccessToken);
            HttpContext.Session.SetString("RefreshToken", response.ResponseModel.JwtToken.RefreshToken);

            return Redirect(RouteEndpoints.AdminDashboard);
        }
    }
}
