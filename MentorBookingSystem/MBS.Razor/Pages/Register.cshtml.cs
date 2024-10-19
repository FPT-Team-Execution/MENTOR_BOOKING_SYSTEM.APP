using MBS.Services.Models.Requests.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MBS.Razor.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty] public RegisterRequest RegisterRequest { get; set; }

        public void OnGet()
        {
        }
    }
}