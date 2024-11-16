using Azure.Core;
using MBS.BusinessObject.Enums;
using MBS.DataAccess.Pagination;
using MBS.Razor.Pages.AdminPage;
using MBS.Repositories.Interfaces;
using MBS.Services.Constants;
using MBS.Services.Dtos;
using MBS.Services.Models.Requests.Group;
using MBS.Services.Services.Implements;
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
        private readonly IStudentService _studentService;

        public Index(IGroupService groupService, IClaimService claimService, IProjectService projectService,
            IRequestService reqService, IMentorService mentorService, IProgressService progressService, IStudentService studentService)
        {
            _groupService = groupService;
            _claimService = claimService;
            _projectService = projectService;
            _reqService = reqService;
            _mentorService = mentorService;
            _progressService = progressService;
            _studentService = studentService;
        }

        public ProjectDto Project { get; set; } = new();
        public List<GroupDto> Groups { get; set; } = new();
        public Pagination<RequestDto> RequestsPagination { get; set; } = new();
        public MentorDto Mentor { get; set; } = new();
        public List<ProgressDto> Progresses { get; set; } = new();

        public double Percent { get; set; } = 0;
        public List<ProgressDto> Complete { get; set; } = new();
        public List<ProgressDto> NotComplete { get; set; } = new();
        
        public Pagination<StudentDto> StudentPagination { get; set; } = new();
        


        //TODO: add search name
        public string SearchName { get; set; } = string.Empty;

        //TODO: filter by request status
        public string SortOrder { get; set; } = "desc";
        public int Size { get; set; } = 5;
        public int PageIndex { get; set; } = 1;

        private async Task GetActiveProjectInfoByUserId(string projectId)
        {
            await LoadStudents();
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
        
        private async Task LoadStudents()
        {
            var students = await _studentService.GetStudentsAsync(page: PageIndex, size: Size, SortOrder);
            StudentPagination = students;

            SaveTempData(TempDataKeys.AdminKeys.StudentPagination, StudentPagination);
            SaveTempData(TempDataKeys.PageIndex, PageIndex);
            SaveTempData(TempDataKeys.PageSize, Size);
            SaveTempData(TempDataKeys.SortOrder, SortOrder);
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
            var studentPagination = GetTempData<Pagination<StudentDto>>(TempDataKeys.AdminKeys.StudentPagination)!;
            //set pageIndex and page Size
            Size = int.Parse(size);
            //if total item from previous load * previous total pages is lower or equal then new size -> pageIndex = 1
            if ((studentPagination.TotalItems * studentPagination.TotalItems) <= Size)
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

    public async Task<IActionResult> OnPostAdd(string id, string projectId)
    {
        await GetActiveProjectInfoByUserId(projectId);
        if (Groups.Count() >= 6)
        {
            SaveTempDataString(TempDataKeys.ErrorMessage, "Max 6 members per group");
            return Page();
        }
        var existGroup = Groups.FirstOrDefault(group => group.StudentId == id );
        if (existGroup != null)
        {
            SaveTempDataString(TempDataKeys.ErrorMessage, "Student already in project");
            return Page();
        }
        CreateNewGroupRequestModel group = new CreateNewGroupRequestModel()
        {
            StudentId = id,
            PositionId = Guid.Parse("D90A1DBA-CC6C-466C-96E5-8EAF98809D8D"),
            ProjectId = Guid.Parse(projectId),
        };
        var result = await _groupService.CreateNewGroupAsync(group);
        if (result)
        {
            SaveTempDataString(TempDataKeys.SuccessMessage, "Group created successfully");
            await GetActiveProjectInfoByUserId(projectId);
        }
        return Page();
    }
    }
}