using System.Security.Claims;
using System.Transactions;
using MBS.BusinessObject.Entities;
using MBS.BusinessObject.Enums;
using MBS.DataAccess.Pagination;
using MBS.Externals.Utils;
using MBS.Razor.Pages.AdminPage;
using MBS.Services.Constants;
using MBS.Services.Dtos;
using MBS.Services.Services.Interfaces;
using MBS.Services.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MBS.Razor.Pages.StudentPage.ProjectPage;

public class Index : BaseAdminPage
{
    private readonly IGroupService _groupService;
    private readonly IClaimService _claimService;
    private readonly IProjectService _projectService;
    private readonly IRequestService _reqService;
    private readonly IMentorService _mentorService;
    private readonly IProgressService _progressService;
    private readonly ICalendarEventService _calendarEventService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IStudentService _studentService;
    private readonly IPointTransactionService _pointTransactionService;
    private readonly IMeetingService _meetingService;
    public Index(
        IGroupService groupService,
        IClaimService claimService,
        IProjectService projectService,
        IRequestService reqService,
        IMentorService mentorService,
        IProgressService progressService,
        ICalendarEventService calendarEventService,
        UserManager<ApplicationUser> userManager,
        IStudentService studentService,
        IPointTransactionService pointTransactionService,
        IMeetingService meetingService
    )
    {
        _groupService = groupService;
        _claimService = claimService;
        _projectService = projectService;
        _reqService = reqService;
        _mentorService = mentorService;
        _progressService = progressService;
        _calendarEventService = calendarEventService;
        _userManager = userManager;
        _studentService = studentService;
        _pointTransactionService = pointTransactionService;
        _meetingService = meetingService;
    }

    public ProjectDto Project { get; set; } = new();
    public List<GroupDto> Groups { get; set; } = new();
    public Pagination<RequestDto> RequestsPagination { get; set; } = new();
    public MentorDto Mentor { get; set; } = new();
    public List<ProgressDto> Progresses { get; set; } = new();

    public double Percent { get; set; } = 0;
    public List<ProgressDto> Complete { get; set; } = new();
    public List<ProgressDto> NotComplete { get; set; } = new();

    [BindProperty]
    public RequestDto Request { get; set; }
    
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
        await LoadRequests(Project.Id);

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

    public async Task LoadRequests(Guid projectId)
    {
        var request = await _reqService.GetRequestsByProjectIdPaginationAsync(projectId, PageIndex, Size, SortOrder);
        var items = request.Items.ToList(); 
        foreach (var requestItem in items)
        {
            if (requestItem.Status == RequestStatusEnum.Accepted.ToString())
            {
                var meetingByRequestId = await _meetingService.GetMeetingByRequestId(requestItem.Id.ToString());
                requestItem.MeetingLink = meetingByRequestId.MeetUp;
            }
        }
        request.Items = items; 

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

    public void OnPostAsync()
    {
    }

    public async Task<IActionResult> OnPostCreateRequest(string title, DateTime start, DateTime end)
    {
        //Check validate
        if (!ModelState.IsValid)
        {
            var invalidEntry = ModelState.First(e => e.Value!.ValidationState == ModelValidationState.Invalid);
            SaveTempDataString(TempDataKeys.ErrorMessage, invalidEntry.Value.Errors.FirstOrDefault()!.ErrorMessage);
            return Page();
        }

        if (start <= DateTime.Now || end <= DateTime.Now || start >= end)
        {
            SaveTempDataString(TempDataKeys.ErrorMessage, "Invalid start and end time!");
            return Page();
        }
        if (start <= DateTime.Now.AddHours(1).Date.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute))
        {
            SaveTempDataString(TempDataKeys.ErrorMessage, "Request is valid from 1 hour later");
            return Page();
        }


        //get project
        var project = GetTempData<ProjectDto>(TempDataKeys.StudentKeys.Project);
        if (project == null || string.IsNullOrEmpty(project.MentorId))
        {
            SaveTempDataString(TempDataKeys.ErrorMessage, "Project not found!");
            return Page();
        }

        //get mentor
        var mentor = GetTempData<MentorDto>(TempDataKeys.StudentKeys.Mentor);
        if (mentor == null || string.IsNullOrEmpty(mentor.Id))
        {
            SaveTempDataString(TempDataKeys.ErrorMessage, "Mentor not found!");
            return Page();
        }

        //get student login
        var studentId = _claimService.GetCookieValue(CookieNames.UserId);
        //check student - creater
        var user = await _userManager.FindByIdAsync(studentId);
        if (user == null)
        {
            return Redirect(RouteEndpoints.Login);
        }
        //Check request overlap
        var requests = await _reqService.GetRequestsByProjectId(project.Id, RequestStatusEnum.Pending.ToString());
        var requestDtos = requests.ToList();
        if (requestDtos.Any())
        {
            foreach (var request in requestDtos)
            {
                if (start < request.End && end > request.Start)
                {
                    SaveTempDataString(TempDataKeys.ErrorMessage, "There is pending request at this time");
                    return Page();
                }
            }
        }
        //check overlap
        var dateRange = ConvertUtils.GetStartEndTime(start);
        var existedEvents = await _calendarEventService.GetCalendarEventsByMentorId(
            mentorId: mentor.Id, 
            startDate: dateRange.Start,
            endDate: dateRange.End);

        var isOverlapped = IsOverlapping(start, end, existedEvents.Where(x => x.Start >= DateTime.Now).ToList());
        if (isOverlapped)
        {
            SaveTempDataString(TempDataKeys.ErrorMessage, "Request time is overlapping.");
            return Page();
        }

        //Check student point
        var groups = await _groupService.GetGroupsByProjectIdAsync(project.Id);
        bool isEnoughPoint = true;
        foreach (var group in groups)
        {
            var student = await _studentService.GetStudentByIdAsync(group.StudentId);
            if (student.WalletPoint < 100)
            {
                isEnoughPoint = false;
                break;
            }
        }

        if (!isEnoughPoint)
        {
            SaveTempDataString(TempDataKeys.ErrorMessage, "Some members not having enough point!");
            return Page();
        }

        //update point
        using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            foreach (var group in groups)
            {
                var student =
                    await _studentService.GetStudentByIdAsync(group.StudentId);
                await _pointTransactionService.ModifyStudentPoint(
                    studentId: student.UserId,
                    amount: 100,
                    transactionType: nameof(TransactionTypeEnum.Debit),
                    kind: nameof(TransactionKindEnum.Project));
            }

            //create request
            var newRequest = new Request()
            {
                Id = Guid.NewGuid(),
                ProjectId = project.Id,
                CreaterId = studentId,
                MentorId = mentor.Id,
                Start = start,
                End = end,
                Title = title,
                Status = RequestStatusEnum.Pending
            };
            var addResult = await _reqService.CreateProjectRequest(newRequest);
            if (!addResult)
            {
                SaveTempDataString(TempDataKeys.ErrorMessage, "Add failed");
                return Page();
            }

            transactionScope.Complete();
        }

        await LoadRequests(project.Id);
        SaveTempDataString(TempDataKeys.SuccessMessage, "Add successfully");
        return Page();
    }


    private bool IsOverlapping(DateTime start, DateTime end, List<CalendarEvent> events)
    {
        foreach (var item in events)
        {
            if (item.Start >= DateTime.Now)
            {
                // Check if the two intervals overlap
                if (start < item.End && end > item.Start)
                {
                    return true;
                }
            }
        }

        return false;
    }
}