using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Services.Models.Responses.CalendarEvent
{
    public class CreateCalendarEventOneFlowResponse
    {
        public string CalendarEventId { get; set; }
        public Guid MeetingId { get; set; }
    }
}
