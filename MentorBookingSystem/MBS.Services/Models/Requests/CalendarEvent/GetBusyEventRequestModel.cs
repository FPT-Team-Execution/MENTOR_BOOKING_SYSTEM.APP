using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Services.Models.Requests.CalendarEvent
{
    public class GetBusyEventRequestModel
    {
        public required string MentorId { get; set; }
        public required DateOnly Day { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
