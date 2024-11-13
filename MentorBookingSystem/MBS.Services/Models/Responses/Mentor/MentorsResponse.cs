using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Services.Models.Responses.Mentor
{
    public class MentorsResponse
    {
        public string Id { get; set; }
        public string FullName { get; set; }

        public string? Email { get; set; }

    }
}
