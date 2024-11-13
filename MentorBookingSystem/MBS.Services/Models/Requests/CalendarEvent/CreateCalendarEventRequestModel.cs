using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Services.Models.Requests.CalendarEvent
{
    public class CreateCalendarRequestModel
    {
        public required string AccessToken { get; set; }
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}$", ErrorMessage = "Date format must be YYYY-MM-DDTHH:MM:SS")]
        public required string Start { get; set; }
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}$", ErrorMessage = "Date format must be YYYY-MM-DDTHH:MM:SS")]
        public required string End { get; set; }
        [MaxLength(450), Required]
        public required string MentorId { get; set; }
        [Required]
        public Guid MeetingId { get; set; }

    }
}
