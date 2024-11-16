using MBS.DataAccess.Pagination;
using MBS.Services.Dtos;
using MBS.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MBS.Razor.Pages.MentorPage.MeetingPage;

public class IndexModel : PageModel
{
    private readonly IMeetingService _meetingService;

    public Pagination<MeetingDto> MeetingPagination { get; set; } = new();
    public int PageSize { get; set; } = 5;
    public int PageIndex { get; set; } = 1;
    public string MentorId { get; set; } = string.Empty;

    public IndexModel(IMeetingService meetingService)
    {
        _meetingService = meetingService;
    }

    public async Task OnGetAsync(string mentorId = "", int pageIndex = 1, int pageSize = 5)
    {
        MentorId = mentorId;
        PageIndex = pageIndex;
        PageSize = pageSize;

        if (!string.IsNullOrWhiteSpace(mentorId))
        {
            MeetingPagination =
                await _meetingService.GetMeetingsByMentorIdPaginationAsync(mentorId, pageIndex, pageSize);
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