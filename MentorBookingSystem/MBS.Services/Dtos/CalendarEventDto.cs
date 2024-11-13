using MBS.BusinessObject.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Services.Dtos
{
    public class CalendarEventDto
    {
        public string Id { get; set; }
        public string HtmlLink { get; set; }
        public string Summary { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string ICalUID { get; set; }
        public Guid? MeetingId { get; set; }
        public Meeting? Meeting { get; set; }
        public string MentorId { get; set; }
        public Mentor Mentor { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public EventStatus Status { get; set; }
    }
}
