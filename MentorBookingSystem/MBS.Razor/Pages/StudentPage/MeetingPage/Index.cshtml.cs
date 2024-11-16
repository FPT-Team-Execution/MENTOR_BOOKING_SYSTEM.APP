using MBS.DataAccess.Pagination;
using MBS.Services.Dtos;
using MBS.Services.Services.Interfaces;
using MBS.Services.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MBS.Razor.Pages.StudentPage.MeetingPage;

public class IndexModel : PageModel
{
    //public void OnGet()
    //{

    //}

    private readonly IMeetingService _meetingService;

    public Pagination<MeetingDto> MeetingPagination { get; set; } = new();
    public int PageSize { get; set; } = 5;
    public int PageIndex { get; set; } = 1;
    public string StudentId { get; set; } = string.Empty;

    public IndexModel(IMeetingService meetingService)
    {
        _meetingService = meetingService;
    }

    public async Task OnGetAsync(string studentId = "", int pageIndex = 1, int pageSize = 5)
    {
        if (string.IsNullOrWhiteSpace(studentId))
        {
            studentId = Request.Cookies[CookieNames.UserId];
        }

        StudentId = studentId;
        PageIndex = pageIndex;
        PageSize = pageSize;

        if (!string.IsNullOrWhiteSpace(studentId))
        {
            MeetingPagination =
                await _meetingService.GetMeetingsByStudentIdPaginationAsync(studentId, pageIndex, pageSize);
        }
        else
        {
            MeetingPagination = new Pagination<MeetingDto>
            {
                Items = new List<MeetingDto>(),
                TotalItems = 0,
                TotalPages = 0,
                PageIndex = 1,
                PageSize = pageSize
            };
        }
    }
}