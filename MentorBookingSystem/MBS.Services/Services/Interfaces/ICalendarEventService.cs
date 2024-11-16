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
        Task<IEnumerable<CalendarEvent>> GetCalendarEventsByMentorId(string mentorId, DateTime startDate, DateTime endDate);
        Task<BaseModel<CreateCalendarResponseModel, CreateCalendarRequestModel>> CreateCalendarEvent(CreateCalendarRequestModel request);
        // Task<BaseModel<Pagination<CalendarEvent>>> GetCalendarEventsByMentorId(string mentorId,string accessToken, GetCalendarEventRequestModel parameters);
        Task<BaseModel<CalendarEventResponseModel>> GetCalendarEventId(string calendarEventId);
        Task<BaseModel<GetBusyEventResponse, GetBusyEventRequestModel>> GetBusyEvent(GetBusyEventRequestModel request);
        Task<BaseModel<UpdateCalendarEventResponseModel>> UpdateCalendarEvent(string calendarEventId, string accessToken, UpdateCalendarEventRequestModel request);
        Task<BaseModel> DeleteCalendarEvent(string calendarEventId);
        Task<BaseModel<CreateCalendarEventOneFlowResponse, CreateCalendarEventOneFlowRequest>> CreateCalendarEventOnelFlow(CreateCalendarEventOneFlowRequest request);

    }
}
