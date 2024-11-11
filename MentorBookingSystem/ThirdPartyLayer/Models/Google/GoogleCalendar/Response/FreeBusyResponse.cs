using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThirdPartyLayer.Models.Google.GoogleCalendar.Response
{
    public class FreeBusyResponse : GoogleResponse
    {
        public string Kind { get; set; }
        public string TimeMin { get; set; }
        public string TimeMax { get; set; }
        public Dictionary<string, CalendarInfo> Calendars { get; set; }
    }

    public class CalendarInfo
    {
        public List<BusySlot> Busy { get; set; }
    }

    public class BusySlot
    {
        public string Start { get; set; }
        public string End { get; set; }
    }
}
