using MBS.Services.Models.Responses.Position;
using MBS.Services.Models.Responses.Project;
using MBS.Services.Models.Responses.Student;
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
        public string ProjectName { get; set; }


        public string StudentName{ get; set; }


        public string PositionName { get; set; }


    }
}

