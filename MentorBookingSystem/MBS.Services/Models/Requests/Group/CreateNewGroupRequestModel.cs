using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Services.Models.Requests.Group
{
    public class CreateNewGroupRequestModel
    {
        public Guid ProjectId { get; set; }
        public string? StudentId { get; set; }
        public Guid PositionId { get; set; }
    }
}
