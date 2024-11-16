using MBS.BusinessObject.Entities;
using MBS.Repositories.Interfaces;
using MBS.Services.Dtos;
using MBS.Services.Models.Requests.CalendarEvent;
using MBS.Services.Services.Implements;
using MBS.Services.Services.Interfaces;
using MBS.Services.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MBS.Razor.Pages.MentorPage.MeetingPage;

public class CreateMeeting : BaseMentorPage
{
    
    private readonly IRequestService _requestService;
    private readonly ICalendarEventService _calendarEventService;

    public CreateMeeting(IRequestService requestService, ICalendarEventService calendarEventService)
    {
        _requestService = requestService;
        _calendarEventService = calendarEventService;
    }
    [BindProperty]
    public CreateCalendarEventOneFlowRequest EventModel { get; set; } = default!;
    [BindProperty]
    public RequestDto RequestInfo { get; set; } = default!; 
    public string message = string.Empty;
    
    public async Task<IActionResult> OnGet(string id) 
    {
        RequestInfo = await _requestService.GetRequestById(Guid.Parse(id));
        return Page();
    }

    public async Task<IActionResult> OnPostCreate()
    {
        if (RequestInfo.Id == Guid.Empty)
        {
            ModelState.AddModelError(string.Empty, "Invalid Request.");
            return Page();
        }

        RequestInfo = await _requestService.GetRequestById(RequestInfo.Id);

        if (RequestInfo == null)
        {
            ModelState.AddModelError(string.Empty, "Request not found.");
            return Page();
        }
        string googleAccessToken = string.Empty;
        HttpContext.Request.Cookies.TryGetValue(CookieNames.GoogleAccessToken, out googleAccessToken);
        EventModel.AccessToken = googleAccessToken;
        EventModel.MentorId = RequestInfo.MentorId;
        EventModel.Start = RequestInfo.Start.ToString("MM/dd/yyyy HH:mm");
        EventModel.End = RequestInfo.End.ToString("MM/dd/yyyy HH:mm");
        EventModel.RequestId = RequestInfo.Id;
        var result = await _calendarEventService.CreateCalendarEventOnelFlow(EventModel);
        if (result.IsSuccess)
        {
            SaveTempDataString(TempDataKeys.SuccessMessage, "Calendar Event Created Successfully");
            message = "Calendar Event Created Successfully";
            return Redirect("/MentorPage/MeetingPage/MeetingDetail?id=" + result.ResponseModel.MeetingId.ToString());
        }
        else
        {
            SaveTempDataString(TempDataKeys.ErrorMessage, result.Message);
            message = result.Message;
        }
       return Page();
    }
}