using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Services.Models.Responses.Meeting
{
    public class MeetingResponse
    {
        public string Id { get; set; }
        public string RequestId { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string MeetUp { get; set; }
        public string Status { get; set; }
    }
}
