using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Services.Models.Requests.Skill
{
    public class CreateNewSkillRequestDTO
    {
        public string Name { get; set; }
        public string MentorId { get; set; }
    }
}
