using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ThirdPartyLayer.Models.Google.GoogleCalendar.Request
{
    public class FreeBusyRequest
    {
        [JsonPropertyName("timeMin")]
        public string TimeMin { get; set; }
        [JsonPropertyName("timeMax")]
        public string TimeMax { get; set; }
        [JsonPropertyName("items")]
        public List<CalendarItem> Items { get; set; }
        [JsonPropertyName("timeZone")]
        public string TimeZone { get; set; }
    }

    public class CalendarItem
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
