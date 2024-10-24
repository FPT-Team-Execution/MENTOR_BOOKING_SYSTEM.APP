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
    //public List<StudentModel> Students { get; set; } = new(); 
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
        var studentModels = data.Items.Select(s => new StudentModel()
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

        //set pagination list
        StudentPagination.PageIndex = data.PageIndex;
        StudentPagination.PageSize = data.PageSize;
        StudentPagination.TotalPages = data.TotalPages;
        StudentPagination.Items = studentModels;
        
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
            var studentsPagination = GetTempData<Pagination<StudentModel>>("studentsPagination");
            StudentPagination = studentsPagination;
            var chosenStudent = studentsPagination.Items.FirstOrDefault(x => x.Id == studentId);
            if (chosenStudent == null)
                SaveTempData("ErrorMessage", "Student not found");
            else
                ChosenStudent = chosenStudent;

        }
        catch (Exception)
        {
            SaveTempData("ErrorMessage", "Some error occurred");
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