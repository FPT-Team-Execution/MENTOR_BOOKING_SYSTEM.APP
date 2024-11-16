using Azure.Core;
using MBS.BusinessObject.Enums;
using MBS.DataAccess.Pagination;
using MBS.Razor.Pages.AdminPage;
using MBS.Services.Constants;
using MBS.Services.Dtos;
using MBS.Services.Services.Interfaces;
using MBS.Services.Shared;
using Microsoft.AspNetCore.Mvc;

namespace MBS.Razor.Pages.AdminPage.ProjectPage.ProjectDetail
{
    public class Index : BaseAdminPage
    {
        private readonly IGroupService _groupService;
        private readonly IClaimService _claimService;
        private readonly IProjectService _projectService;
        private readonly IRequestService _reqService;
        private readonly IMentorService _mentorService;
        private readonly IProgressService _progressService;

        public Index(IGroupService groupService, IClaimService claimService, IProjectService projectService,
            IRequestService reqService, IMentorService mentorService, IProgressService progressService)
        {
            _groupService = groupService;
            _claimService = claimService;
            _projectService = projectService;
            _reqService = reqService;
            _mentorService = mentorService;
            _progressService = progressService;
        }

        public ProjectDto Project { get; set; } = new();
        public List<GroupDto> Groups { get; set; } = new();
        public Pagination<RequestDto> RequestsPagination { get; set; } = new();
        public MentorDto Mentor { get; set; } = new();
        public List<ProgressDto> Progresses { get; set; } = new();

        public double Percent { get; set; } = 0;
        public List<ProgressDto> Complete { get; set; } = new();
        public List<ProgressDto> NotComplete { get; set; } = new();


        //TODO: add search name
        public string SearchName { get; set; } = string.Empty;

        //TODO: filter by request status
        public string SortOrder { get; set; } = "desc";
        public int Size { get; set; } = 5;
        public int PageIndex { get; set; } = 1;

        private async Task GetActiveProjectInfoByUserId(string projectId)
        {
            //Get activated project information 
            var project = await _projectService.GetProjectByIdAsync(Guid.Parse(projectId));
            if (project == null)
            {
                SaveTempDataString(TempDataKeys.ErrorMessage, "No information found for this project");
                return;
            }

            //Get all members in project
            var members = await _groupService.GetGroupsByProjectIdAsync(Guid.Parse(projectId));
            //Save data to binding object
            Groups = (List<GroupDto>)members;
            Project = project;
            //Save to next use
            SaveTempData(TempDataKeys.StudentKeys.Groups, Groups);
            SaveTempData(TempDataKeys.StudentKeys.Project, Project);

            //*get request by project id
            var request =
                await _reqService.GetRequestsByProjectIdPaginationAsync(Guid.Parse(projectId), PageIndex, Size,
                    SortOrder);
            RequestsPagination = request;

            SaveTempData(TempDataKeys.StudentKeys.RequestPagination, RequestsPagination);
            SaveTempData(TempDataKeys.PageIndex, PageIndex);
            SaveTempData(TempDataKeys.PageSize, Size);
            SaveTempData(TempDataKeys.SortOrder, SortOrder);

            //* get processes by process
            var progresses = await _progressService.GetProgressByProjectIdAsync(Project.Id);
            Progresses = progresses.ToList();
            SaveTempData(TempDataKeys.StudentKeys.Progresses, Progresses);


            //* get processes detail
            var progressDetail = await _progressService.GetCompleteProgressPercent(Project.Id);
            Percent = progressDetail.Percent;
            SaveTempData(TempDataKeys.StudentKeys.Percent, Percent);
            Complete = progressDetail.Complete.ToList();
            SaveTempData(TempDataKeys.StudentKeys.Complete, Complete);
            NotComplete = progressDetail.NotComplete.ToList();
            SaveTempData(TempDataKeys.StudentKeys.NotComplete, NotComplete);


            //* get mentor info
            var mentor = await _mentorService.GetMentorById(project.MentorId);
            if (mentor == null)
            {
                SaveTempDataString(TempDataKeys.ErrorMessage, "No information found for this project");
                return;
            }

            Mentor = mentor;
            SaveTempData(TempDataKeys.StudentKeys.Mentor, Mentor);
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            try
            {
                await GetActiveProjectInfoByUserId(id);
            }
            catch
            {
                SaveTempDataString(TempDataKeys.ErrorMessage, "Some error occurred");
                return RedirectToPage(RouteEndpoints.AdminStudent);
            }

            return Page();
        }
    }
}