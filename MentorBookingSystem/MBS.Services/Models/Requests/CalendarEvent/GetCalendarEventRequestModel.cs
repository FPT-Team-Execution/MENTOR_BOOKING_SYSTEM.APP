using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Services.Models.Requests.CalendarEvent
{
    public class GetCalendarEventRequestModel
    {
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}$", ErrorMessage = "Date format must be YYYY-MM-DDTHH:MM:SS")]
        public required string StartTime { get; set; }
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}$", ErrorMessage = "Date format must be YYYY-MM-DDTHH:MM:SS")]
        public required string EndTime { get; set; }
        public required string SortBy { get; set; } = "asc";
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
    }
}
