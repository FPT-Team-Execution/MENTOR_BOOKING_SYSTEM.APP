using Mapster;
using MBS.Razor.Pages.AdminPage.StudentPage.Models;
using MBS.Services.Constants;
using MBS.Services.Models;
using MBS.Services.Models.Responses.Major;
using MBS.Services.Models.Responses.Student;
using MBS.Services.Services.Interfaces;
using MBS.Services.Utils;
using Microsoft.AspNetCore.Mvc;

namespace MBS.Razor.Pages.AdminPage.StudentPage;

public class Index : BaseAdminPage
{
    public Pagination<StudentModel> StudentPagination { get; set; } = new();
    public List<MajorResponse> Majors { get; set; } = new();
    [BindProperty] public StudentModel ChosenStudent { get; set; } = new();

    public string SortOrder { get; set; } = "asc";
    public string SearchName { get; set; } = string.Empty;

    private readonly IStudentService _studentService;
    private readonly IMajorService _majorService;
    public Index(IStudentService studentService, IMajorService majorService)
    {
        _studentService = studentService;
        _majorService = majorService;
    }

    private async Task LoadMajors()
    {
        var data = (await _majorService.GetMajorsAsync(1, 100) as BaseModel<Pagination<MajorResponse>>).ResponseRequestModel.Items;
        var majorModels = data.Adapt<IEnumerable<MajorResponse>>();
        Majors = majorModels.ToList();
        SaveTempData(TempDataKeys.Majors, Majors);
    }
    private async Task LoadStudents()
    {
        var data = await _studentService.GetStudentsAsync(page: 1, size: 10, "asc");
        var studentModels = data.Adapt<Pagination<StudentModel>>();
        StudentPagination = studentModels;
        SaveTempData(TempDataKeys.StudentPagination, StudentPagination);
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            await LoadStudents();
            await LoadMajors();

        }
        catch
        {
            SaveTempData(TempDataKeys.ErrorMessage, "Some error occurred");
            return RedirectToPage(RouteEndpoints.AdminStudent);
        }

        return Page(); 
    }


    public async Task<IActionResult> OnGetShowStudentDetail(string studentId)
    {
        try
        {
            StudentPagination = GetTempData<Pagination<StudentModel>>(TempDataKeys.StudentPagination)!;
            var chosenStudent = StudentPagination.Items.FirstOrDefault(x => x.Id == studentId);
            SaveTempData(TempDataKeys.ChosenStudent, chosenStudent);
            if (chosenStudent == null)
                SaveTempDataString(TempDataKeys.ErrorMessage, "Student not found");
            else
                ChosenStudent = chosenStudent;

        }
        catch (Exception)
        {
            SaveTempDataString(TempDataKeys.ErrorMessage, "Some error occurred");
            Redirect(RouteEndpoints.AdminStudent);
        }
        return Page();
    }
    public async Task<IActionResult> OnPostSearch(string searchName, string sortOrder)
    {
        StudentPagination = GetTempData<Pagination<StudentModel>>(TempDataKeys.StudentPagination)!;
        if (!string.IsNullOrEmpty(searchName))
        {
            var query = StudentPagination.Items.Where(s => s.FullName.Contains(searchName));
            query = sortOrder == "asc" ? query.OrderBy(x => x.FullName) : query.OrderByDescending(x => x.FullName);
            SaveTempData(TempDataKeys.StudentPagination, query.ToList());
        }
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

    public IActionResult OnPostDelete(string studentId)
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
            return OnPostDelete(chosenStudent.Id);
        }
        return Page();
    }
    
}