using MBS.Services.Models.Requests.Auth;
using MBS.Services.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MBS.Razor.Pages
{
    public class LoginModel : PageModel
    {
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
