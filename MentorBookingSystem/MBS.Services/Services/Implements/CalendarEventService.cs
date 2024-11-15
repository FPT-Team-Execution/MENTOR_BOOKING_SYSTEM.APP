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
using MBS.BusinessObject.Entities;
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


        public Task<BaseModel<CreateCalendarResponseModel, CreateCalendarRequestModel>> CreateCalendarEvent(CreateCalendarRequestModel request)
        {
            throw new NotImplementedException();
        }

        public Task<BaseModel<Pagination<CalendarEvent>>> GetCalendarEventsByMentorId(string mentorId, string accessToken, GetCalendarEventRequestModel parameters)
        {
            throw new NotImplementedException();
        }

        public Task<BaseModel<CalendarEventResponseModel>> GetCalendarEventId(string calendarEventId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseModel<GetBusyEventResponse, GetBusyEventRequestModel>> GetBusyEvent(GetBusyEventRequestModel request)
        {
            throw new NotImplementedException();
        }

        public Task<BaseModel<UpdateCalendarEventResponseModel>> UpdateCalendarEvent(string calendarEventId, string accessToken, UpdateCalendarEventRequestModel request)
        {
            throw new NotImplementedException();
        }

        public Task<BaseModel> DeleteCalendarEvent(string calendarEventId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseModel<CreateCalendarEventOneFlowResponse, CreateCalendarEventOneFlowRequest>> CreateCalendarEventOnelFlow(CreateCalendarEventOneFlowRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
