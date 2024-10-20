using MBS.Services.Constants;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MBS.Razor.Pages;

public class Logout : PageModel
{
    public void OnGet()
    {
        HttpContext.Session.Clear();
        Response.Redirect(RouteEndpoints.Login);
    }
}