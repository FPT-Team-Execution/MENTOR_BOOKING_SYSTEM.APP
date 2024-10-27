using MBS.Razor.Pages.AdminPage.StudentPage.Models;
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
        var response = await _projectService.GetProjectAsync(PageIndex, Size, search) as BaseModel<Pagination<ProjectResponse>>;
        var projectList = response!.ResponseRequestModel.Adapt<Pagination<ProjectModel>>();
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
}