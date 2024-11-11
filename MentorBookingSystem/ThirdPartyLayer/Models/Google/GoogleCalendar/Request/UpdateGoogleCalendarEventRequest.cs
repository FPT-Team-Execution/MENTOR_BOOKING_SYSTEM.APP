using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThirdPartyLayer.Models.Google.GoogleCalendar.Request
{
    public class UpdateGoogleCalendarEventRequest
    {
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public DateTime End { get; set; }
        public string TimeZone { get; set; }
    }
}
