using MBS.Razor.Pages.AdminPage.StudentPage.Models;
using MBS.Services.Constants;
using MBS.Services.Models.Responses.Student;
using MBS.Services.Services.Implements;
using MBS.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MBS.Razor.Pages.AdminPage.StudentPage;

public class Index : PageModel
{
    public StudentModel ChosenStudent { get; set; } = new();
    [BindProperty] public List<StudentModel> Students { get; set; } = new();
    private readonly IStudentService _studentService;
    public Index(IStudentService studentService)
    {
        _studentService = studentService;
    }

    public async Task OnGet()
    {
        await LoadStudents();
    }

    public async Task<IActionResult> OnGetShowStudentDetail(string studentId)
    {
        await LoadStudents();

        var chosenStudent = Students.FirstOrDefault(x => x.Id == studentId);
        if (chosenStudent == null)
            TempData["ErrorMessage"] = "Student not found";
        else
            ChosenStudent = chosenStudent;

        return Page();
    }

    private async Task LoadStudents()
    {
        var data = await _studentService.GetStudentsAsync(page: 1, size: 10);
        Students = data.Items.Select(s => new StudentModel()
        {
            Id = s.Id,
            FullName = s.FullName,
            Email = s.Email,
            University = s.University,
            MajorName = s.MajorId,
            WalletPoint = s.WalletPoint,
            AvatarUrl = s.AvatarUrl,
            Gender = s.Gender,
            Birthday = s.Birthday,
            LockoutEnabled = s.LockoutEnabled,
            UserName = s.UserName,
            PhoneNumber = s.PhoneNumber,
            EmailConfirmed = s.EmailConfirmed,
            LockoutEnd = s.LockoutEnd,
            CreatedBy = s.CreatedBy,
            CreatedOn = s.CreatedOn,
            UpdatedBy = s.UpdatedBy,
            UpdatedOn = s.UpdatedOn
        })
        .OrderBy(s => s.FullName)
        .ToList();

        // Store students in session
        //HttpContext.Session.SetString("Students", JsonSerializer.Serialize(Students));
    }
}