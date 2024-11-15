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
using System.Transactions;
using MBS.BusinessObject.Entities;
using MBS.BusinessObject.Enums;
using MBS.Externals.Models.Google;
using MBS.Externals.Models.Google.GoogleCalendar.Request;
using MBS.Externals.Models.Google.GoogleCalendar.Response;
using MBS.Externals.Models.Google.GoogleMeeting.Response;
using MBS.Externals.Services.Interfaces;
using MBS.Externals.Utils;
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

        public async Task<BaseModel<Pagination<CalendarEvent>>> GetCalendarEventsByMentorId(string mentorId, string accessToken, GetCalendarEventRequestModel parameters)
        {
            try
        {
            var startDatetime = DateTime.Parse(parameters.StartTime);
            var endDatetime = DateTime.Parse(parameters.EndTime);

            //check time
            if (startDatetime >= endDatetime)
            {
                return new BaseModel<Pagination<CalendarEvent>>
                {
                    Message = "Invalid parameters start time or end time.",
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    ResponseRequestModel = null,
                };
            }
            //check mentor
            var mentor = await _mentorRepository.GetMentorByIdAsync(mentorId);
            if (mentor == null)
            {
                return new BaseModel<Pagination<CalendarEvent>>
                {
                    Message = "User not found",
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    ResponseRequestModel = null,
                };
            }
            //find events by mentor
            var events = await _calendarEventRepository.GetCalendarEventsByMentorIdPaginationAsync(
                mentorId, startDatetime, endDatetime, parameters.SortBy!, parameters.Page, parameters.Size);
            //get events from google calendar
            var gRequest = new GetGoogleCalendarEventsRequest
            {
                Email = mentor.User.Email!,
                AccessToken = "Invalid google access token",
                TimeMin = startDatetime,
                TimeMax = endDatetime,
            };
            var googleResponse = await _googleService.ListEvents(gRequest);
            if (!googleResponse.IsSuccess)
            {
                return new BaseModel<Pagination<CalendarEvent>>
                {
                    Message = ((GoogleErrorResponse)googleResponse).Error.Message,
                    IsSuccess = false,
                    StatusCode = ((GoogleErrorResponse)googleResponse).Error.Code,
                    ResponseRequestModel = null
                };
            }
            // filter new and old
            var newEventsFromGoogle = FilterNewEvents(mentorId, (List<CalendarEvent>)events.Items, ((GetGoogleCalendarEventsResponse)googleResponse).Items);
            if (newEventsFromGoogle.Any())
            {
                var addRangeResult = await _calendarEventRepository.CreateRangeAsync(newEventsFromGoogle);
                if (!addRangeResult)
                {
                    return new BaseModel<Pagination<CalendarEvent>>
                    {
                        Message = "Create event fail",
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status500InternalServerError,
                        ResponseRequestModel = null,
                    };
                }
            }
            var asyncEvents = await _calendarEventRepository.GetCalendarEventsByMentorIdPaginationAsync(
                mentorId, startDatetime, endDatetime, parameters.SortBy, parameters.Page, parameters.Size);
            return new BaseModel<Pagination<CalendarEvent>>
            {
                Message ="Create event successfully",
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel = asyncEvents
            };
        }
        catch (Exception e)
        {
            return new BaseModel<Pagination<CalendarEvent>>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
                ResponseRequestModel = null,
            };
        }
        }

        private List<CalendarEvent> FilterNewEvents(string mentorId, List<CalendarEvent> localEvents, List<GoogleCalendarEvent> googleEvents)
        {
            // Initialize dictionary with local events
            var localEventDictionary = localEvents.ToDictionary(e => e.Id, e => e);

            // Filter new events from Google Calendar that don't exist in the local events
            var newEvents = googleEvents
                .Where(googleEvent => !localEventDictionary.ContainsKey(googleEvent.Id))
                .Select(googleEvent => new CalendarEvent
                {
                    Id = googleEvent.Id,
                    HtmlLink = googleEvent.HtmlLink,
                    Summary = googleEvent.Summary,
                    Description = string.Empty,
                    ICalUID = googleEvent.ICalUID,
                    Created = googleEvent.Created,
                    Updated = googleEvent.Updated,
                    MeetingId = null,
                    MentorId = mentorId,
                    Start = googleEvent.Start.DateTime,
                    End = googleEvent.End.DateTime,
                    Status = (EventStatus)Enum.Parse(typeof(EventStatus), googleEvent.Status)
                })
                .ToList();

            return newEvents;
        }
        
        public async Task<BaseModel<CalendarEventResponseModel>> GetCalendarEventId(string calendarEventId)
        {
            try
            {
                var calendarEvent = await _calendarEventRepository.GetByIdAsync(calendarEventId, "Id");
                if (calendarEvent == null)
                    return new BaseModel<CalendarEventResponseModel>
                    {
                        Message = "Not found for calendar event",
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status404NotFound,
                        ResponseRequestModel = null
                    };

                return new BaseModel<CalendarEventResponseModel>
                {
                    Message = "Get calendar event successfully",
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    ResponseRequestModel = new CalendarEventResponseModel
                    {
                        CalendarEvent = calendarEvent
                    }
                };
            }
            catch (Exception e)
            {
                return new BaseModel<CalendarEventResponseModel>
                {
                    Message = e.Message,
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ResponseRequestModel = null,
                };
            }
        }

        public async Task<BaseModel<GetBusyEventResponse, GetBusyEventRequestModel>> GetBusyEvent(GetBusyEventRequestModel request)
        {
            try
            {
                var mentor = await _mentorRepository.GetByIdAsync(request.MentorId, "UserId");
                if (mentor == null)
                {
                    return new BaseModel<GetBusyEventResponse, GetBusyEventRequestModel>
                    {
                        Message = "User not found",
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status404NotFound,
                    };
                }
                var (start, end) = ConvertUtils.GetStartEndTime(request.Day);
                var events = await _calendarEventRepository.GetCalendarEventsByMentorIdAsync(request.MentorId, start, end);
                var busyEventsInDay = events.Adapt<IEnumerable<BusyEventModel>>();
                return new BaseModel<GetBusyEventResponse, GetBusyEventRequestModel>
                {
                    Message = "Get busy events successfully",
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    RequestModel = request,
                    ResponseModel = new GetBusyEventResponse
                    {
                        Events = busyEventsInDay.ToList()
                    }
                };
            }
            catch (Exception e)
            {
                return new BaseModel<GetBusyEventResponse, GetBusyEventRequestModel>
                {
                    Message = e.Message,
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
            }
        }

        public async Task<BaseModel<UpdateCalendarEventResponseModel>> UpdateCalendarEvent(string calendarEventId, string accessToken, UpdateCalendarEventRequestModel request)
        {
            try
        {
            //check meeting Id
            var meeting = await _meetingRepository.GetByIdAsync(request.MeetingId, "Id");
            if (meeting == null)
                return new BaseModel<UpdateCalendarEventResponseModel>
                {
                    Message = "Meeting not found",
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    ResponseRequestModel = null,

                };
            if (meeting.Status != MeetingStatusEnum.New)
                return new BaseModel<UpdateCalendarEventResponseModel>
                {
                    Message = "Invalid meeting required",
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    ResponseRequestModel = null,

                };

            //update calendar event
            var calendarEvent = await _calendarEventRepository.GetEventByIdAsync(calendarEventId);
            if (calendarEvent == null)
                return new BaseModel<UpdateCalendarEventResponseModel>
                {
                    Message = "Not found for calendar event",
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    ResponseRequestModel = null,

                };
            //TODO: call google calendar api to recheck event props
            //~
            var updateGEventRequets = new UpdateGoogleCalendarEventRequest()
            {
                Start = request.Start.Value,
                End = request.End.Value,
                TimeZone = "Asia/Ho_Chi_Minh"
            };

            var googleUpdateResponse = await _googleService.UpdateEvent(
                eventId: calendarEventId,
                email: calendarEvent.Mentor.UserId,
                accessToken: accessToken,
                updateRequest: updateGEventRequets
                );
            if (!googleUpdateResponse.IsSuccess)
                return new BaseModel<UpdateCalendarEventResponseModel>
                {
                    Message = ((GoogleErrorResponse)googleUpdateResponse).Error.Message,
                    IsSuccess = false,
                    StatusCode = ((GoogleErrorResponse)googleUpdateResponse).Error.Code,
                    ResponseRequestModel = null
                };
            GoogleCalendarEvent googleCalendarEventUpdated = (GoogleCalendarEvent)googleUpdateResponse;
            //update local events
            calendarEvent.HtmlLink = googleCalendarEventUpdated.HtmlLink;
            calendarEvent.Description = request.Description;
            calendarEvent.Summary = googleCalendarEventUpdated.Summary;
            calendarEvent.ICalUID = googleCalendarEventUpdated.ICalUID;
            calendarEvent.Updated = googleCalendarEventUpdated.Updated;
            // if (request.Start != null)
            //     calendarEvent.Start = request.Start.Value;
            // if (request.End != null)
            //     calendarEvent.End = request.End.Value;
            calendarEvent.Start = googleCalendarEventUpdated.Start.DateTime;
            calendarEvent.End = googleCalendarEventUpdated.End.DateTime;
            calendarEvent.MeetingId = request.MeetingId;
            var updateResult = _calendarEventRepository.Update(calendarEvent);
            if (updateResult)
                return new BaseModel<UpdateCalendarEventResponseModel>
                {
                    Message = "Update event successfully",
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    ResponseRequestModel = new UpdateCalendarEventResponseModel
                    {
                        Event = calendarEvent,
                    }
                };
            return new BaseModel<UpdateCalendarEventResponseModel>
            {
                Message = "Update event failed",
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
                ResponseRequestModel = null,
            };
        }
        catch (Exception e)
        {
            return new BaseModel<UpdateCalendarEventResponseModel>
            {
                Message = "Update event failed",
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
                ResponseRequestModel = null,
            };
        }
        }

        public async Task<BaseModel> DeleteCalendarEvent(string calendarEventId)
        {
            try
            {
                //check meeting status related to canlendar event ~ Cancled
                var calendarEvent =
                    await _calendarEventRepository.GetEventByIdAsync(calendarEventId);
                if (calendarEvent == null)
                    return new BaseModel
                    {
                        Message = "Not found for calendar event",
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status404NotFound,
                    };
                // if (calendarEvent.Meeting!.Status != MeetingStatusEnum.Canceled)
                //     return new BaseModel<DeleteCalendarEventResponseModel>
                //     {
                //         Message = MessageResponseHelper.InvalidMeetingSatus(calendarEvent.MeetingId.ToString()),
                //         IsSuccess = false,
                //         StatusCode = StatusCodes.Status400BadRequest,
                //     };

                //update calendarEvent to cancled (deleted)
                calendarEvent.Status = EventStatus.Cancleled;
                var updateResult = _calendarEventRepository.Update(calendarEvent);
                if (updateResult)
                    return new BaseModel
                    {
                        Message = "",
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status204NoContent,
                    };
                return new BaseModel
                {
                    Message = "Delete calendar event fail",
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
            catch (Exception e)
            {
                return new BaseModel
                {
                    Message = e.Message,
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
            }
        }

        public async Task<BaseModel<CreateCalendarEventOneFlowResponse, CreateCalendarEventOneFlowRequest>> CreateCalendarEventOnelFlow(CreateCalendarEventOneFlowRequest request)
        {
             try
        {
            //* get  request 
            var requestFound = await _requestRepository.GetRequestById(request.RequestId);

            if (requestFound == null)
                return new BaseModel<CreateCalendarEventOneFlowResponse, CreateCalendarEventOneFlowRequest>
                {
                    Message = "Not found for reuqest Id",
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };

            if (requestFound.Status != RequestStatusEnum.Pending)
                return new BaseModel<CreateCalendarEventOneFlowResponse, CreateCalendarEventOneFlowRequest>
                {
                    Message = "Invalid request status",
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                };
            var startDatetime = DateTime.Parse(request.Start);
            var endDatetime = DateTime.Parse(request.End);

            //check mentorId
            var mentor = await _mentorRepository.GetMentorByIdAsync(request.MentorId);
            if (mentor == null)
            {
                return new BaseModel<CreateCalendarEventOneFlowResponse, CreateCalendarEventOneFlowRequest>
                {
                    Message = "User not found",
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
                return new BaseModel<CreateCalendarEventOneFlowResponse, CreateCalendarEventOneFlowRequest>
                {
                    Message = "Is overlayed",
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                };
            }
            //* create event on google calendar
            var createGEventRequest = new CreateGoogleCalendarEventRequest()
            {
                Summary = $"Meeting {(request.IsOnline ? "ONLINE" : "OFFLINE")}",
                Description = $"You have meeting with project: {requestFound.Project.Title.ToUpper()}",
                Start = startDatetime,
                End = endDatetime,
                TimeZone = "Asia/Ho_Chi_Minh"
            };
            var googleEventResponse = await _googleService.InsertEventWithGoogleMeetCreate(
                email: mentor.User.Email,
                accessToken: request.AccessToken,
                createRequest: createGEventRequest,
                location: request.Location,
                isOnline: request.IsOnline
                );
            if (!googleEventResponse.IsSuccess)
            {
                return new BaseModel<CreateCalendarEventOneFlowResponse, CreateCalendarEventOneFlowRequest>
                {
                    Message = ((GoogleErrorResponse)googleEventResponse).Error.Message,
                    IsSuccess = false,
                    StatusCode = ((GoogleErrorResponse)googleEventResponse).Error.Code
                };
            }
            var googleEvent = ((GoogleCalendarEvent)googleEventResponse);

            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                //update request to accepted
                requestFound.Status = RequestStatusEnum.Accepted;
                _requestRepository.Update(requestFound);
                //*create meeting 
                var googleMeetingUrl = string.Empty;
                if (request.IsOnline)
                {
                    GoogleResponse googleMeetingResponse = await _googleService.CreateMeeting(request.AccessToken);
                    if (!googleMeetingResponse.IsSuccess)
                        return new BaseModel<CreateCalendarEventOneFlowResponse, CreateCalendarEventOneFlowRequest>
                        {
                            Message = ((GoogleErrorResponse)googleMeetingResponse).Error.Message,
                            IsSuccess = false,
                            StatusCode = ((GoogleErrorResponse)googleMeetingResponse).Error.Code
                        };
                    googleMeetingUrl = ((GoogleMeetingResponse)googleMeetingResponse).MeetingUri;
                }
                var newMeeting = new Meeting()
                {
                    Id = Guid.NewGuid(),
                    RequestId = requestFound.Id,
                    Description = request.Description,
                    Location = googleEvent.Location,
                    MeetUp = googleMeetingUrl,
                    Status = MeetingStatusEnum.New
                };
                //create meeting
                await _meetingRepository.CreateAsync(newMeeting);
                //add new calendar event
                var eventCreate = new CalendarEvent()
                {
                    Id = googleEvent.Id,
                    Status = (EventStatus)Enum.Parse(typeof(EventStatus), googleEvent.Status, ignoreCase: true),
                    Description = $"You have meeting with project: {requestFound.Project.Title.ToUpper()}",
                    HtmlLink = googleEvent.HtmlLink,
                    Created = googleEvent.Created,
                    Updated = googleEvent.Updated,
                    Summary = googleEvent.Summary,
                    ICalUID = googleEvent.ICalUID,
                    Start = googleEvent.Start.DateTime,
                    End = googleEvent.End.DateTime,
                    MentorId = request.MentorId,
                    MeetingId = newMeeting.Id,
                };
                var addResult = await _calendarEventRepository.CreateAsync(eventCreate);
                if (addResult)
                {
                    transactionScope.Complete();
                    return new BaseModel<CreateCalendarEventOneFlowResponse, CreateCalendarEventOneFlowRequest>
                    {
                        Message = "Create event successfully",
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status200OK,
                        RequestModel = request,
                        ResponseModel = new CreateCalendarEventOneFlowResponse
                        {
                            CalendarEventId = eventCreate.Id,
                            MeetingId = newMeeting.Id
                        }
                    };
                }
                
                return new BaseModel<CreateCalendarEventOneFlowResponse, CreateCalendarEventOneFlowRequest>
                {
                    Message = "Create event failed",
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
            }
                
        }
        catch (Exception e)
        {
            return new BaseModel<CreateCalendarEventOneFlowResponse, CreateCalendarEventOneFlowRequest>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
        }
    }
}
