using MBS.Razor.Pages.AdminPage.StudentPage.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MBS.Razor.Pages.AdminPage.StudentPage;

public class StudentDetailPartial : PageModel
{
    public StudentModel? ChosenStudent { get; set; }

    //public StudentDetailPartial(StudentModel? chosenStudent)
    //{
    //    ChosenStudent = chosenStudent;
    //}
    public void OnGet(string studentId)
    {
        if (string.IsNullOrEmpty(studentId))
        {
            return;
        }
    }
}