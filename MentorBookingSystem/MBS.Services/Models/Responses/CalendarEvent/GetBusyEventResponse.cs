using MBS.Services.Models.Requests.CalendarEvent;

namespace MBS.Services.Models.Responses.CalendarEvent;

public class GetBusyEventResponse
{
    public List<BusyEventModel> Events { get; set; } = [];
}