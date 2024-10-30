using Mapster;
using MBS.Razor.Pages.AdminPage.StudentPage.Models;
using MBS.Services.Constants;
using MBS.Services.Models;
using MBS.Services.Models.Requests.Student;
using MBS.Services.Models.Responses.Major;
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
    
    public int Size { get; set; } = 5;
    public int PageIndex { get; set; } = 1;
    

    private readonly IStudentService _studentService;
    private readonly IMajorService _majorService;

    public Index(IStudentService studentService, IMajorService majorService)
    {
        _studentService = studentService;
        _majorService = majorService;
    }

    private async Task LoadMajors()
    {
        var data = (await _majorService.GetMajorsAsync(1, 100) as BaseModel<Pagination<MajorResponse>>)
            .ResponseRequestModel.Items;
        var majorModels = data.Adapt<IEnumerable<MajorResponse>>();
        Majors = majorModels.ToList();
        SaveTempData(TempDataKeys.AdminKeys.Majors, Majors);
    }

    private async Task LoadStudents()
    {
        var data = await _studentService.GetStudentsAsync(page: PageIndex, size: Size, SortOrder);
        var studentModels = data.Adapt<Pagination<StudentModel>>();
        StudentPagination = studentModels;
        
        SaveTempData(TempDataKeys.AdminKeys.StudentPagination, StudentPagination);
        SaveTempData(TempDataKeys.PageIndex, PageIndex);
        SaveTempData(TempDataKeys.PageSize, Size);
        SaveTempData(TempDataKeys.SortOrder, SortOrder);

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
            StudentPagination = GetTempData<Pagination<StudentModel>>(TempDataKeys.AdminKeys.StudentPagination)!;
            var chosenStudent = StudentPagination.Items.FirstOrDefault(x => x.Id == studentId);
            SaveTempData(TempDataKeys.AdminKeys.ChosenStudent, chosenStudent);
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
        try
        {
            //Set Sort variables
            SearchName = searchName;
            SortOrder = sortOrder;
            //Get Page Size and Page Index (if exist)
            var pageSizeData = GetTempData<string>(TempDataKeys.PageSize);
            if (pageSizeData != null && int.TryParse(pageSizeData, out int pageSize))
                Size = pageSize;
            var pageIndexData = GetTempData<string>(TempDataKeys.PageIndex);
            if (pageIndexData != null && int.TryParse(pageIndexData, out int pageIndex))
                PageIndex = pageIndex;
            //Load data
            await LoadStudents();
        
            var query = StudentPagination.Items.AsQueryable();

            if (!string.IsNullOrEmpty(SearchName))
            {
                var words = searchName.Split(" ");
                //* All() => all condition true from words in order to return true for where
                query = query.Where(s => words.All(c => s.FullName.ToLower().Contains(c.ToString().ToLower())));
            }
            StudentPagination.Items = query.ToList();
            //* modify total pages based on item
            StudentPagination.PageSize = Size;
            StudentPagination.PageIndex = StudentPagination.TotalPages < PageIndex ? 1 : PageIndex;
            //Save temp data to next use
            SaveTempData(TempDataKeys.SortOrder, SortOrder);
            SaveTempData(TempDataKeys.SearchName, SearchName);
            SaveTempData(TempDataKeys.AdminKeys.StudentPagination, StudentPagination);
        }
        catch (Exception e)
        {
            SaveTempDataString(TempDataKeys.ErrorMessage, "Some error occurred");
            Redirect(RouteEndpoints.AdminStudent);
        }
        

        return Page();
    }

    public async Task<IActionResult> OnPostPageNavigate(string pageIndex, string size)
    {
        try
        {
            var studentPagination = GetTempData<Pagination<StudentModel>>(TempDataKeys.AdminKeys.StudentPagination)!;
            //set pageIndex and page Size
            Size = int.Parse(size);
            //if total item from previous load * previous total pages is lower or equal then new size -> pageIndex = 1
            if((studentPagination.TotalItems * studentPagination.TotalItems) <= Size)
                PageIndex = 1;
            else
                PageIndex = int.Parse(pageIndex);
            //Save temp data to next use
            SaveTempData(TempDataKeys.PageIndex, PageIndex);
            SaveTempData(TempDataKeys.PageSize, Size);
            //Load data pagination from api
            await LoadStudents();
          
        }
        catch (Exception e)
        {
            SaveTempDataString(TempDataKeys.ErrorMessage, "Some error occurred");
            Redirect(RouteEndpoints.AdminStudent);
        }
        return Page();
    }

    public async Task<IActionResult> OnPostCreate()
    {
        return await OnGetAsync();
    }

    public async Task<IActionResult> OnPutUpdate(StudentModel student)
    {
        var studentModelRequest = student.Adapt<UpdateStudentRequest>();
        var data = await _studentService.UpdateStudentAsync(studentModelRequest);
        if (!data.IsSuccess)
        {
            SaveTempDataString(TempDataKeys.ErrorMessage, data.Message);
            return await OnGetShowStudentDetail(student.Id);
        }
        //Load data
        await LoadStudents();
        SaveTempDataString(TempDataKeys.SuccessMessage, "Update Successful");
        SaveTempData(TempDataKeys.AdminKeys.ChosenStudent, student);
        return await OnGetShowStudentDetail(student.Id);
    }
    
    public async Task<IActionResult> OnPost(StudentModel chosenStudent, string action)
    {
        try
        {
            switch (action)
            {
                case "create":
                    return await OnPostCreate();
                case "update":
                    return await OnPutUpdate(chosenStudent);
                default:
                    return Page();
            }
        }
        catch (Exception e)
        {
            SaveTempDataString(TempDataKeys.ErrorMessage, "Some error occurred");
            return Redirect(RouteEndpoints.AdminStudent);
        }
    }
}