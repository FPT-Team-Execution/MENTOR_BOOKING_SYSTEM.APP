using MBS.BusinessObject.Entities;
using MBS.Services.Models.Requests.CalendarEvent;
using MBS.Services.Models.Responses.CalendarEvent;
using MBS.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBS.Services.Dtos;
using MBS.DataAccess.Pagination;

namespace MBS.Services.Services.Interfaces
{
    public interface ICalendarEventService
    {
        Task<CalendarEventDto> CreateCalendarEvent(CreateCalendarRequestModel request);
        Task<BaseModel<Pagination<CalendarEventDto>>> GetCalendarEventsByMentorId(string mentorId, string accessToken, GetCalendarEventsRequestModel parameters);
        Task<BaseModel<CalendarEventDto>> GetCalendarEventId(string calendarEventId);
        Task<BaseModel<IEnumerable<CalendarEventDto>>> GetBusyEvent(GetBusyEventRequestModel request);
        Task<BaseModel<CalendarEventDto>> UpdateCalendarEvent(string calendarEventId, string accessToken, UpdateCalendarEventRequestModel request);
        Task<BaseModel> DeleteCalendarEvent(string calendarEventId);
        Task<BaseModel<CreateCalendarEventOneFlowResponse, CreateCalendarEventOneFlowRequest>> CreateCalendarEventOnelFlow(CreateCalendarEventOneFlowRequest request);
        Task<IEnumerable<CalendarEventDto>> GetAllCalendarEvents();

    }
}
