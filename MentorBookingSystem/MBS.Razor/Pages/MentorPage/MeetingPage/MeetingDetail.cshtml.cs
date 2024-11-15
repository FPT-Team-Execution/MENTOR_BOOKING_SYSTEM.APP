using MBS.Repositories.Interfaces;
using MBS.Services.Dtos;
using MBS.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MBS.Razor.Pages.MentorPage;

public class MeetingDetail : PageModel
{
    
    private readonly IMeetingService _meetingService;
    private readonly IRequestService _requestService;

    public MeetingDetail(IMeetingService meetingService, IRequestService requestService)
    {
        _meetingService = meetingService;
        _requestService = requestService;
    }
    
    private RequestDto RequestInfo { get; set; }
    private MeetingDto MeetingInfo { get; set; }
    
    
    
    public async Task<IActionResult> OnGet(string id)
    {
        RequestInfo = await _requestService.GetRequestById(Guid.Parse(id));
        if (RequestInfo == null)
        {
            return NotFound();
        }
        
        
        return Page();
        
    }
}