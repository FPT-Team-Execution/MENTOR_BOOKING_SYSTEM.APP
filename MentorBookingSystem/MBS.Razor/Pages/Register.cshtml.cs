using System.Threading.Tasks;
using MBS.Services.Constants;
using MBS.Services.Models;
using MBS.Services.Models.Requests.Auth;
using MBS.Services.Models.Responses.Major;
using MBS.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MBS.Razor.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IAuthService _authService;
        private readonly IMajorService _majorService;

        public BaseModel<Pagination<GetMajorResponse>>? MajorData { get; set; }

        public RegisterModel(IAuthService authService, IMajorService majorService)
        {
            _authService = authService;
            _majorService = majorService;
        }

        [BindProperty] public RegisterRequest RegisterRequest { get; set; }

        public async Task OnGet()
        {
            MajorData = await _majorService.GetMajorsAsync(1, 100) as BaseModel<Pagination<GetMajorResponse>>;
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                MajorData = await _majorService.GetMajorsAsync(1, 100) as BaseModel<Pagination<GetMajorResponse>>;
                return Page();
            }

            var response = await _authService.RegisterAsync(RegisterRequest);

            if (response.StatusCode != StatusCodes.Status200OK)
            {
                TempData["ErrorMessage"] = response.Message;
                MajorData = await _majorService.GetMajorsAsync(1, 100) as BaseModel<Pagination<GetMajorResponse>>;
                return Page();
            }

            TempData["SuccessMessage"] = response.Message;
            return Redirect(RouteEndpoints.Login);
        }
    }
}