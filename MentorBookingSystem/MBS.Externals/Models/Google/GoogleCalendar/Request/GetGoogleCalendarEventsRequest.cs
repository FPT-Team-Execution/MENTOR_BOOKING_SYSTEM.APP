using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Externals.Models.Google.GoogleCalendar.Request
{
    public class GetGoogleCalendarEventsRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string AccessToken { get; set; }
        [Required]
        public DateTime? TimeMax { get; set; }
        [Required]
        public DateTime? TimeMin { get; set; }
    }
    public class FreeBusyParamters
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string AccessToken { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime Day { get; set; }
    }
}
