using MBS.Razor.Pages.AdminPage;
using MBS.Services.Constants;
using MBS.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MBS.Razor.Pages;

public class ConfirmEmail : BaseAdminPage
{
    private readonly IAuthService _authService;

    public ConfirmEmail(IAuthService authService)
    {
        _authService = authService;
    }

    public void OnGet()
    {
    }

    [BindProperty] public string Token { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        var email = GetTempData<string>("Email");
        var user = await _authService.GetUserByEmailAsync(email);

        if (user is null)
        {
            TempData["ErrorMessage"] = "User was not found!";
            return Page();
        }

        var result = await _authService.VerifyEmail(user, Token);

        if (!result)
        {
            TempData["ErrorMessage"] = "Confirm email fail!";
            return Page();
        }

        SaveTempDataString("SuccessMessage", "Confirm email successfully");
        return Redirect(RouteEndpoints.Login);
    }
}