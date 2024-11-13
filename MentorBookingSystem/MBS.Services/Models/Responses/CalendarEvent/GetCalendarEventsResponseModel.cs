using MBS.BusinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Services.Models.Responses.CalendarEvents
{
    public class GetCalendarEventsResponseModel
    {
        public IEnumerable<MBS.BusinessObject.Entities.CalendarEvent> Events { get; set; }
    }
}
