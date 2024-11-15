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

    public IndexModel(IMeetingService meetingService)
    {
        _meetingService = meetingService;
    }

    public async Task OnGetAsync(int pageIndex = 1, int pageSize = 5)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;

        MeetingPagination = await _meetingService.GetMeetingsPaginationAsync(pageIndex, pageSize);
    }
}