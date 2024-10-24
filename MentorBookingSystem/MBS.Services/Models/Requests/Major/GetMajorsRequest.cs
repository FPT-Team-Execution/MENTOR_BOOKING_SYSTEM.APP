using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Services.Models.Requests.Major
{
    internal class GetMajorsRequest
    {
        public int Page { get; set; }
        public int Size { get; set; }
    }
}
