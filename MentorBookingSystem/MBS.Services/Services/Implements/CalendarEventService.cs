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
using MBS.Externals.Models.Google.GoogleCalendar.Request;
using MBS.Externals.Models.Google.GoogleCalendar.Response;
using MBS.Externals.Services.Interfaces;
using Microsoft.AspNetCore.Http;

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


        public async Task<BaseModel<CreateCalendarResponseModel, CreateCalendarRequestModel>> CreateCalendarEvent(CreateCalendarRequestModel request)
        {
            try
        {
            //* get project from meeting Id
            var meetingDetail = await _meetingRepository.GetByIdAsync(request.MeetingId, "Id");

            if (meetingDetail == null)
                return new BaseModel<CreateCalendarResponseModel, CreateCalendarRequestModel>
                {
                    Message = "Meeting detail null for now" ,
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };

            //* find project by meetingId
            var project = await _requestRepository.GetRequestById(meetingDetail.RequestId);

            var startDatetime = DateTime.Parse(request.Start);
            var endDatetime = DateTime.Parse(request.End);

            //check mentorId
            var mentor = await _mentorRepository.GetMentorByIdAsync(request.MentorId);
            if (mentor == null)
            {
                return new BaseModel<CreateCalendarResponseModel, CreateCalendarRequestModel>
                {
                    Message = "Mentor null for now",
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            }

            //find overlayed events
            var freeBusyRequest = new FreeBusyParamters()
            {
                Email = mentor.User.Email,
                AccessToken = request.AccessToken,
                Day = startDatetime,
            };

            var freeBusyResponse = await _googleService.GetFreeBusyPeriod(freeBusyRequest);

            var isOverlayed = IsOverlapping(startDatetime, endDatetime, ((FreeBusyResponse)freeBusyResponse).Calendars[mentor.User.Email].Busy);

            if (isOverlayed)
            {
                return new BaseModel<CreateCalendarResponseModel, CreateCalendarRequestModel>
                {
                    Message = "Is over layed for now",
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                };
            }
            //create event on google calendar
            var createGEventRequest = new CreateGoogleCalendarEventRequest()
            {
                Summary = "Meeting",
                Description = $"You have meeting with project: {project.Title.ToUpper()}",
                Start = startDatetime,
                End = endDatetime,
                TimeZone = "Asia/Ho_Chi_Minh"
            };
            var googleEventResponse = await _googleService.InsertEvent(mentor.User.Email, request.AccessToken, createGEventRequest);
            if (!googleEventResponse.IsSuccess)
            {
                return new BaseModel<CreateCalendarResponseModel, CreateCalendarRequestModel>
                {
                    Message = ((GoogleErrorResponse)googleEventResponse).Error.Message,
                    IsSuccess = false,
                    StatusCode = ((GoogleErrorResponse)googleEventResponse).Error.Code
                };
            }

            var googleEvent = ((GoogleCalendarEvent)googleEventResponse);
            //add new calendar event
            var eventCreate = new CalendarEvent()
            {
                Id = googleEvent.Id,
                Status = (EventStatus)Enum.Parse(typeof(EventStatus), googleEvent.Status, ignoreCase: true),
                Description = $"You have meeting with project: {project.Title.ToUpper()}",
                HtmlLink = googleEvent.HtmlLink,
                Created = googleEvent.Created,
                Updated = googleEvent.Updated,
                Summary = googleEvent.Summary,
                ICalUID = googleEvent.ICalUID,
                Start = googleEvent.Start.DateTime,
                End = googleEvent.End.DateTime,
                MentorId = request.MentorId,
                MeetingId = request.MeetingId,
            };
            var addResult = await _calendarEventRepository.CreateAsync(eventCreate);
            if (addResult)
                return new BaseModel<CreateCalendarResponseModel, CreateCalendarRequestModel>
                {
                    Message = "Create event successfully",
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    RequestModel = request,
                    ResponseModel = new CreateCalendarResponseModel
                    {
                        CalendarEventId = eventCreate.Id,
                    }
                };
            return new BaseModel<CreateCalendarResponseModel, CreateCalendarRequestModel>
            {
                Message = "Create event failed",
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };

        }
        catch (Exception e)
        {
            return new BaseModel<CreateCalendarResponseModel, CreateCalendarRequestModel>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
        }
        
        private bool IsOverlapping(DateTime start, DateTime end, List<BusySlot> busySlots)
        {
            // Convert the start and end times to DateTimeOffset, assuming they are in the same timezone (no offset)
            DateTimeOffset startDateTimeOffset = new DateTimeOffset(start);
            DateTimeOffset endDateTimeOffset = new DateTimeOffset(end);

            foreach (var slot in busySlots)
            {
                // Parse the end and start times of the busy slot using DateTimeOffset to account for time zone
                DateTimeOffset busyStartTime = DateTimeOffset.Parse(slot.Start);
                DateTimeOffset busyEndTime = DateTimeOffset.Parse(slot.End);

                // Check if the two intervals overlap
                // An overlap occurs when start is before the busy slot end time and end is after the busy slot start time
                if (startDateTimeOffset < busyEndTime && endDateTimeOffset > busyStartTime)
                {
                    return true;
                }
            }
            return false;
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
