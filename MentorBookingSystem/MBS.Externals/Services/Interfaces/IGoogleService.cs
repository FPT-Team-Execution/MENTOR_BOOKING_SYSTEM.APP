using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MBS.Externals.Models.Google.GoogleCalendar.Request;
using MBS.Externals.Models.Google;
using Microsoft.AspNetCore.Http;

namespace MBS.Externals.Services.Interfaces
{
    public interface IGoogleService
    {
        // String GenerateOauthUrl();
        //* Google Auth
        Task<GoogleResponse> AuthenticateGoogleUserAsync(HttpContext context);
        Task<GoogleResponse> GetTokenGoogleUserAsync(string authenticatedCode, string externalCallbackUri);
        Task<GoogleResponse> GetProfileGoogleUserAsync(string accessToken);

        //* Google Calendar
        Task<GoogleResponse> ListEvents(GetGoogleCalendarEventsRequest getRequest);
        Task<GoogleResponse> InsertEvent(string email, string accessToken, CreateGoogleCalendarEventRequest createRequest);
        Task<GoogleResponse> UpdateEvent(string email, string eventId, string accessToken, UpdateGoogleCalendarEventRequest updateRequest);
        Task<GoogleResponse> DeleteEvent(string email, string eventId, string accessToken);
        Task<GoogleResponse> GetFreeBusyPeriod(FreeBusyParamters request);
        Task<GoogleResponse> CreateMeeting(string accessToken);
        Task<GoogleResponse> InsertEventWithGoogleMeetCreate(string email, string accessToken, string location, CreateGoogleCalendarEventRequest createRequest, bool isOnline = false);

    }
}
