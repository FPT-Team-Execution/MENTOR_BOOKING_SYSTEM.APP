using System.Security.Claims;
using MBS.Services.Constants;
using MBS.Services.Constants.Enums;
using MBS.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MBS.Razor.Pages;

public class DeniedPage : PageModel
{
    private readonly IClaimService _claimService;
    public DeniedPage(IClaimService claimService)
    {
        _claimService = claimService;
    }
    public void OnGetGoBack()
    {
        var claims = _claimService.GetClaims();
        var roleClaim = claims[ClaimTypes.Role] ?? "";
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