using MBS.BusinessObject.Enums;
using MBS.DataAccess.Pagination;
using MBS.Razor.Pages.AdminPage;
using MBS.Services.Constants;
using MBS.Services.Dtos;
using MBS.Services.Services.Interfaces;
using MBS.Services.Utils.Shared;
using Microsoft.AspNetCore.Mvc;

namespace MBS.Razor.Pages.StudentPage.ProjectPage;

public class Index : BaseAdminPage
{
    private readonly IGroupService _groupService;
    private readonly IClaimService _claimService;
    private readonly IProjectService _projectService;
    private readonly IRequestService _reqService;

    public Index(IGroupService groupService, IClaimService claimService, IProjectService projectService, IRequestService reqService)
    {
        _groupService = groupService;
        _claimService = claimService;
        _projectService = projectService;
        _reqService = reqService;
    }

    public ProjectDto Project { get; set; } = new();
    public List<GroupDto> Groups { get; set; } = new();
    public Pagination<RequestDto> RequestsPagination { get; set; } = new();
    
    //TODO: add search name
    public string SearchName { get; set; } = string.Empty;
    //TODO: filter by request status
    public string SortOrder { get; set; } = "desc";
    public int Size { get; set; } = 5;
    public int PageIndex { get; set; } = 1;

    private async Task GetActiveProjectInfoByUserId(string userId)
    {
        //Get all groups that student has joined
        var userGroups = await _groupService.GetGroupsByStudentIdAsync(userId, ProjectStatusEnum.Activated.ToString());
        var group = userGroups.FirstOrDefault();
        if (group == null)
        {
            SaveTempDataString(TempDataKeys.ErrorMessage, "Student has joined no group");
            return;
        }

        //Get activated project information 
        var project = await _projectService.GetProjectByIdAsync(group.ProjectId);
        if (project == null)
        {
            SaveTempDataString(TempDataKeys.ErrorMessage, "No information found for this project");
            return;
        }

        //Get all members in project
        var members = await _groupService.GetGroupsByProjectIdAsync(group.ProjectId);
        //Save data to binding object
        Groups = (List<GroupDto>)members;
        Project = project;
        //Save to next use
        SaveTempData(TempDataKeys.StudentKeys.Groups, Groups);
        SaveTempData(TempDataKeys.StudentKeys.Project, Project);

        //*get request by project id
        var request = await _reqService.GetRequestsByProjectIdPaginationAsync(group.ProjectId, PageIndex, Size, SortOrder);
        RequestsPagination = request;
        
        SaveTempData(TempDataKeys.StudentKeys.RequestPagination, RequestsPagination);
        SaveTempData(TempDataKeys.PageIndex, PageIndex);
        SaveTempData(TempDataKeys.PageSize, Size);
        SaveTempData(TempDataKeys.SortOrder, SortOrder);
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var userIdClaim = _claimService.GetCookieValue(CookieNames.UserId);
            await GetActiveProjectInfoByUserId(userIdClaim);
        }
        catch
        {
            SaveTempDataString(TempDataKeys.ErrorMessage, "Some error occurred");
            return RedirectToPage(RouteEndpoints.AdminStudent);
        }

        return Page();
    }
}