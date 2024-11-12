using System.Threading.Tasks;
using MBS.BusinessObject.Commom;
using MBS.BusinessObject.Entities;
using MBS.Services.Constants;
using MBS.Services.Constants.Enums;
using MBS.Services.Dtos;
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
        private readonly IStudentService _studentService;

        public IEnumerable<MajorDto> MajorData { get; set; }

        public RegisterModel(IAuthService authService, IMajorService majorService, IStudentService studentService)
        {
            _authService = authService;
            _majorService = majorService;
            _studentService = studentService;
        }

        [BindProperty] public RegisterRequest RegisterRequest { get; set; }

        public async Task OnGet()
        {
            MajorData = await _majorService.GetAllMajors();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                MajorData = await _majorService.GetAllMajors();
                return Page();
            }

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = RegisterRequest.Email,
                FullName = RegisterRequest.FullName,
                Gender = RegisterRequest.Gender,
            };

            var createUserResult = await _authService.CreateUserAsync(user, RegisterRequest.Password);

            if (!createUserResult)
            {
                TempData["ErrorMessage"] = "Register fail!";
                MajorData = await _majorService.GetAllMajors();
                return Page();
            }


            var student = new StudentDto()
            {
                MajorId = RegisterRequest.MajorId,
                UserId = user.Id,
                University = RegisterRequest.University,
                WalletPoint = 100,
            };

            var createStudentResult = await _studentService.CreateStudentAsync(student);

            if (string.IsNullOrEmpty(createStudentResult))
            {
                TempData["ErrorMessage"] = "Register fail!";
                MajorData = await _majorService.GetAllMajors();
                return Page();
            }

            var addToRoleResult = await _authService.AddToRoleAsync(user, UserRole.Student);

            if (!addToRoleResult)
            {
                TempData["ErrorMessage"] = "Register fail!";
                MajorData = await _majorService.GetAllMajors();
                return Page();
            }

            TempData["SuccessMessage"] = "Register successfully";
            return Redirect(RouteEndpoints.Login);
        }
    }
}