using System.Security.Claims;
using MBS.Services.Constants;
using MBS.Services.Constants.Enums;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MBS.Razor.Pages;

public class DeniedPage : PageModel
{
    public void OnGetGoBack()
    {
        var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        switch (roleClaim)
        {
            case UserRole.Admin:
                Response.Redirect(RouteEndpoints.AdminDashboard);
                break;
            case UserRole.Mentor:
                Response.Redirect(RouteEndpoints.Mentor);
                break;
            case UserRole.Student:
                Response.Redirect(RouteEndpoints.Student);
                break;
            default: 
                Response.Redirect(RouteEndpoints.Login);
                break;
        }
    }
}