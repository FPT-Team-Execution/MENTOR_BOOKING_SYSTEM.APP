using MBS.BusinessObject.Entities;
using MBS.Repositories.Interfaces;
using MBS.Services.Models.Requests.CalendarEvent;
using MBS.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MBS.Razor.Pages.MentorPage.MeetingPage;

public class CreateMeeting : PageModel
{
    
    private readonly IRequestService _requestService;
    private readonly ICalendarEventService _calendarEventService;

    public CreateMeeting(IRequestService requestService, ICalendarEventService calendarEventService)
    {
        _requestService = requestService;
        _calendarEventService = calendarEventService;
    }
    [BindProperty]
    public CreateCalendarEventOneFlowRequest eventModel { get; set; } = default!;
    public Request Request { get; set; } = default!; 
    
    public void OnGet(string id) 
    {
        
    }
}