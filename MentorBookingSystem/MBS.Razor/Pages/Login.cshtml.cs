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

        public void OnPost()
        {
            
        }
    }
}
