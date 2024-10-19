using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MBS.Razor.Pages;

public class LoginGoogle : PageModel
{
    public void OnGet()
    {
        Response.Redirect("https://localhost:7554/api/auth/google/signin");
    }
}