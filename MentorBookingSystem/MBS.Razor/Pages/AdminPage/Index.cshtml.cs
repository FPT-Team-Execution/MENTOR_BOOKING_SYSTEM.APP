using MBS.Services.Constants;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MBS.Razor.Pages.AdminPage;

public class Index : PageModel
{
    public void OnGet()
    {
        Response.Redirect(RouteEndpoints.AdminDashboard);
    }
}