using MBS.Razor.Pages.AdminPage;
using MBS.Services.Constants;
using MBS.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MBS.Razor.Pages;

public class ResendConfirmEmail : BaseAdminPage
{
    private readonly IClaimService _claimService;
    private readonly IAuthService _authService;

    [BindProperty] public string Email { get; set; }

    public ResendConfirmEmail(IClaimService claimService, IAuthService authService)
    {
        _claimService = claimService;
        _authService = authService;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var email = _claimService.GetCookieValue("USER_EMAIL");
        var user = await _authService.GetUserByEmailAsync(email);

        if (user is null)
        {
            TempData["ErrorMessage"] = "User was not found!";
            return Redirect(RouteEndpoints.ConfirmEmail);
        }

        if (user.EmailConfirmed)
        {
            TempData["SuccessMessage"] = "Email is confirmed";
            return Redirect(RouteEndpoints.Login);
        }

        await _authService.SendVerifyEmail(user);
        return Redirect(RouteEndpoints.ConfirmEmail);
    }
}