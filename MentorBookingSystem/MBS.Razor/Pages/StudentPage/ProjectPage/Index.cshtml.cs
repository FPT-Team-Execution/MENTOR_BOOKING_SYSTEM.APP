using System.Security.Claims;
using Mapster;
using MBS.BusinessObject.Entities;
using MBS.BusinessObject.Enums;
using MBS.Razor.Pages.AdminPage;
using MBS.Repositories.Interfaces;
using MBS.Services.Constants;
using MBS.Services.Dtos;
using MBS.Services.Services.Interfaces;
using MBS.Services.Utils;
using Microsoft.AspNetCore.Mvc;

namespace MBS.Razor.Pages.StudentPage.ProjectPage;

public class Index : BaseAdminPage
{
    private readonly IGroupService _groupService;
    private readonly IClaimService _claimService;
    private readonly IProjectService _projectService;
    public Index(IGroupService groupService, IClaimService claimService, IProjectService projectService)
    {
        _groupService = groupService;
        _claimService = claimService;
        _projectService = projectService;
    }

    public ProjectDto Project { get; set; } = new();
    public List<GroupDto> Groups { get; set; } = new();
    public string SortOrder { get; set; } = "asc";
    public string SearchName { get; set; } = string.Empty;
    public int Size { get; set; } = 5;
    public int PageIndex { get; set; } = 1;
    
    private async Task GetActiveProjectByUserId(string userId)
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
    }

    public async Task<IActionResult> OnGet()
    {
        try
        {
            var userIdClaim = _claimService.GetClaims().ContainsKey(ClaimTypes.NameIdentifier)
                ? _claimService.GetClaims()[ClaimTypes.NameIdentifier]
                : "";
            var roleClaim = _claimService.GetClaims().ContainsKey(ClaimTypes.Role)
                ? _claimService.GetClaims()[ClaimTypes.Role]
                : "";
            await GetActiveProjectByUserId(userIdClaim);
        }
        catch
        {
            SaveTempDataString(TempDataKeys.ErrorMessage, "Some error occurred");
            return RedirectToPage(RouteEndpoints.AdminStudent);
        }
        return Page();
    }
}