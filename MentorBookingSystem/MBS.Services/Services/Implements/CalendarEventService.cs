using Mapster;
using MBS.DataAccess.Pagination;
using MBS.Repositories.Implements;
using MBS.Repositories.Interfaces;
using MBS.Services.Constants;
using MBS.Services.Dtos;
using MBS.Services.Models;
using MBS.Services.Models.Requests.CalendarEvent;
using MBS.Services.Models.Requests.Major;
using MBS.Services.Models.Requests.Mentor;
using MBS.Services.Models.Responses.CalendarEvent;
using MBS.Services.Models.Responses.Group;
using MBS.Services.Models.Responses.Mentor;
using MBS.Services.Services.Interfaces;
using MBS.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBS.Externals.Services.Interfaces;

namespace MBS.Services.Services.Implements
{

    public class CalendarEventService : ICalendarEventService
    {   
        private readonly ICalendarEventRepository _calendarEventRepository;
        private readonly IMentorRepository _mentorRepository;
        private readonly IRequestRepository _requestRepository;
        private readonly IMeetingRepository _meetingRepository;
        private readonly IGoogleService _googleService;

        public CalendarEventService(ICalendarEventRepository calendarEventRepository, IMentorRepository mentorRepository, IRequestRepository requestRepository, IMeetingRepository meetingRepository, IGoogleService googleService)
        {
            _calendarEventRepository = calendarEventRepository;
            _mentorRepository = mentorRepository;
            _googleService = googleService;
            _meetingRepository = meetingRepository;
            _requestRepository = requestRepository;
        }

        public async Task<CalendarEventDto> CreateCalendarEvent(CreateCalendarRequestModel request)
        {
            var result = await WebUtils.PostAsync(
                ApiEndPoints.MajorUrl,
                request,
                token: WebUtils.AccessToken
                );
            return result.Adapt<CalendarEventDto>();
        }


        public Task<BaseModel<CreateCalendarEventOneFlowResponse, CreateCalendarEventOneFlowRequest>> CreateCalendarEventOnelFlow(CreateCalendarEventOneFlowRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<BaseModel> DeleteCalendarEvent(string calendarEventId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CalendarEventDto>> GetAllCalendarEvents()
        {
            var resultSet = await _calendarEventRepository.GetAllAsync();
            return resultSet.Adapt<IEnumerable<CalendarEventDto>>();
        }

        public Task<BaseModel<IEnumerable<CalendarEventDto>>> GetBusyEvent(GetBusyEventRequestModel request)
        {
            throw new NotImplementedException();
        }

        public Task<BaseModel<CalendarEventDto>> GetCalendarEventId(string calendarEventId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseModel<Pagination<CalendarEventDto>>> GetCalendarEventsByMentorId(string mentorId, string accessToken, GetCalendarEventsRequestModel parameters)
        {
            throw new NotImplementedException();
        }

        public Task<BaseModel<CalendarEventDto>> UpdateCalendarEvent(string calendarEventId, string accessToken, UpdateCalendarEventRequestModel request)
        {
            throw new NotImplementedException();
        }

        

    }
}
