using MBS.Services.Models.Responses.Major;
using MBS.Services.Models;
using MBS.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MBS.Razor.Pages.AdminPage.ProjectPage.Models;
using MBS.Services.Constants;
using MBS.Services.Services.Implements;
using MBS.Services.Utils;
using MBS.Services.Models.Responses.Project;
using Mapster;
using MBS.Razor.Pages.AdminPage.MentorPage.Models;
using MBS.Services.Utils.Shared;
using MBS.DataAccess.Pagination;

namespace MBS.Razor.Pages.AdminPage.ProjectPage;

public class Index : BaseAdminPage
{
    public Pagination<ProjectModel> ProjectPagination { get; set; } = new();
    [BindProperty] public ProjectModel ChosenProject { get; set; } = new();

    public string SortOrder { get; set; } = "asc";
    public string search { get; set; } = "";

    public int Size { get; set; } = 5;
    public int PageIndex { get; set; } = 1;


    private readonly IProjectService _projectService;

    public Index(IProjectService projectService)
    {
        _projectService = projectService;
    }

    private async Task LoadProject()
    {
        var response = await _projectService.GetProjectAsync(PageIndex, Size, search);
        var projectList = response.Adapt<Pagination<ProjectModel>>();
        ProjectPagination = projectList;

        SaveTempData(TempDataKeys.AdminKeys.ProjectPagination, ProjectPagination);
        SaveTempData(TempDataKeys.PageIndex, PageIndex);
        SaveTempData(TempDataKeys.PageSize, Size);

    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            await LoadProject();
        }
        catch
        {
            SaveTempData(TempDataKeys.ErrorMessage, "Some error occurred");
            return RedirectToPage(RouteEndpoints.AdminProject);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostPageNavigate(string pageIndex, string size)
    {
        try
        {
            var projectPagination = GetTempData<Pagination<ProjectModel>>(TempDataKeys.AdminKeys.ProjectPagination)!;
            //set pageIndex and page Size
            Size = int.Parse(size);
            //if total item from previous load * previous total pages is lower or equal then new size -> pageIndex = 1
            if ((projectPagination.TotalItems * projectPagination.TotalItems) <= Size)
                PageIndex = 1;
            else
                PageIndex = int.Parse(pageIndex);
            //Save temp data to next use
            SaveTempData(TempDataKeys.PageIndex, PageIndex);
            SaveTempData(TempDataKeys.PageSize, Size);
            //Load data pagination from api
            await LoadProject();
        }
        catch (Exception e)
        {
            SaveTempDataString(TempDataKeys.ErrorMessage, "Some error occurred");
            Redirect(RouteEndpoints.AdminStudent);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostSearch(string search)
    {
        try
        {
            this.search = search;
            SaveTempData("search", search);
            //Save temp data to next use
            SaveTempData(TempDataKeys.PageIndex, 1);
            SaveTempData(TempDataKeys.PageSize, 2);
            //Load data pagination from api
            await LoadProject();
        }
        catch (Exception e)
        {
            SaveTempDataString(TempDataKeys.ErrorMessage, "Some error occurred");
            Redirect(RouteEndpoints.AdminStudent);
        }

        return Page();
    }
}