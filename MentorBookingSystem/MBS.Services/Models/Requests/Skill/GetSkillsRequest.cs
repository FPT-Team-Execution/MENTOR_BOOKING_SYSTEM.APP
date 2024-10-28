using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Services.Models.Requests.Skill
{
    public class GetSkillsRequest
    {
        public int Page { get; set; }
        public int Size { get; set; }
    }
}
