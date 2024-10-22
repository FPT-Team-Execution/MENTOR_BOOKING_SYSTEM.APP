using MBS.Razor.Pages.AdminPage.StudentPage.Models;
using MBS.Services.Models.Responses.Student;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MBS.Razor.Pages.AdminPage.StudentPage;

public class Index : PageModel
{
    public StudentModel ChosenStudent { get; set; } = new();
    public List<StudentModel> Students { get; set; } = new List<StudentModel>()
    {
        new StudentModel()
        {
            Id = "1",
            FullName = "John Doe",
            Email = "john.doe@example.com",
            University = "Example University",
            MajorName = "Computer Science",
            WalletPoint = 100,
            AvatarUrl = "https://example.com/avatar1.jpg",
            Gender = "Male",
            Birthday = new DateTime(2000, 1, 1),
            LockoutEnabled = false,
            UserName = "johndoe",
            PhoneNumber = "123-456-7890",
            EmailConfirmed = true,
            LockoutEnd = null,
            CreatedBy = "admin",
            CreatedOn = DateTime.Now,
            UpdatedBy = null,
            UpdatedOn = null
        },
        new StudentModel()
        {
            Id = "2",
            FullName = "Jane Smith",
            Email = "jane.smith@example.com",
            University = "Example University",
            MajorName = "Mathematics",
            WalletPoint = 150,
            AvatarUrl = "https://example.com/avatar2.jpg",
            Gender = "Female",
            Birthday = new DateTime(2001, 2, 2),
            LockoutEnabled = true,
            UserName = "janesmith",
            PhoneNumber = "987-654-3210",
            EmailConfirmed = false,
            LockoutEnd = DateTime.Now.AddDays(30),
            CreatedBy = "admin",
            CreatedOn = DateTime.Now,
            UpdatedBy = null,
            UpdatedOn = null
        }
    };
    public void OnGet()
    {
        
    }

    public async Task<IActionResult> OnGetShowStudentDetail(string studentId)
    {
        var chosenStudent = Students.FirstOrDefault(x => x.Id == studentId);
        if (chosenStudent == null)
            TempData["ErrorMessage"] = "Student not found";
        else
            ChosenStudent = chosenStudent;
        return Page();
    }
}