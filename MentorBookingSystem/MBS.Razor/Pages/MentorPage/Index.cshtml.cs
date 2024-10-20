using MBS.Services.Constants;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MBS.Razor.Pages.MentorPage;

public class Index : PageModel
{
    public void OnGet()
    {
        Response.Redirect(RouteEndpoints.MentorMeeting);
    }
}