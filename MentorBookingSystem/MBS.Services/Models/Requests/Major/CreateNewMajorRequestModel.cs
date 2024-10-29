using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Services.Models.Requests.Major
{
    public class CreateNewMajorRequestModel
    {
        public string MajorName { get; set; }
        public Guid ParentId{ get; set; }

    }
}
