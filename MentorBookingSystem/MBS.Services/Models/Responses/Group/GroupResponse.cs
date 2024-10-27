using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Services.Models.Responses.Group
{
    public class GroupResponse
    {
        public Guid ProjectId { get; set; }

        public string StudentId { get; set; }

        public Guid PositionId { get; set; }

    }
}

