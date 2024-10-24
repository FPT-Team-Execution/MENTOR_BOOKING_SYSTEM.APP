using Mapster;
using MBS.Razor.Pages.AdminPage.StudentPage.Models;
using MBS.Services.Constants;
using MBS.Services.Models;
using MBS.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace MBS.Razor.Pages.AdminPage.StudentPage;

public class Index : BaseAdminPage
{
    public Pagination<StudentModel> StudentPagination { get; set; } = new();
    [BindProperty] public StudentModel ChosenStudent { get; set; } = new();

    public string SortOrder { get; set; } = "asc";
    public string SearchName { get; set; }

    private readonly IStudentService _studentService;
    public Index(IStudentService studentService)
    {
        _studentService = studentService;
    }
    private async Task LoadStudents()
    {
        var data = await _studentService.GetStudentsAsync(page: 1, size: 10, "asc");
        var studentModels = data.Adapt<Pagination<StudentModel>>();
        StudentPagination = studentModels;
        
        SaveTempData("StudentsPagination", StudentPagination);
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            await LoadStudents();
            
        }
        catch
        {
            SaveTempData("ErrorMessage", "Some error occurred");
            return RedirectToPage(RouteEndpoints.Login);
        }

        return Page(); 
    }


    public async Task<IActionResult> OnGetShowStudentDetail(string studentId)
    {
        try
        {
            StudentPagination = GetTempData<Pagination<StudentModel>>("StudentsPagination")!;
            var chosenStudent = StudentPagination.Items.FirstOrDefault(x => x.Id == studentId);
            if (chosenStudent == null)
                SaveTempDataString("ErrorMessage", "Student not found");
            else
                ChosenStudent = chosenStudent;

        }
        catch (Exception)
        {
            SaveTempDataString("ErrorMessage", "Some error occurred");
            Redirect(RouteEndpoints.Login);
        }
        return Page();
    }
    public async Task<IActionResult> OnPostSearch(string searchName, string sortOrder)
    {
        await LoadStudents();
        StudentPagination.Items = StudentPagination.Items.Where(x => x.FullName.Contains(searchName));
        return Page();
    }
    
    public IActionResult OnPostCreate()
    {
        // Thực hiện logic tạo sinh viên mới
        return RedirectToPage("/Success");
    }

    public IActionResult OnPostUpdate()
    {
        // Thực hiện logic cập nhật sinh viên
        return RedirectToPage("/Success");
    }

    public IActionResult OnPostDelete()
    {
        // Thực hiện logic xóa sinh viên
        return RedirectToPage("/Success");
    }

    public IActionResult OnPost(StudentModel chosenStudent, string action)
    {
        var action2 = Request.Form["action"];
        if (action == "create")
        {
            return OnPostCreate();
        }
        if (action == "update")
        {
            return OnPostUpdate();
        }
        if (action == "delete")
        {
            return OnPostDelete();
        }
        return Page();
    }
    
}