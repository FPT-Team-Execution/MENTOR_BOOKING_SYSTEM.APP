using MBS.BusinessObject.Commom;
using MBS.BusinessObject.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.BusinessObject.Entities
{
    public class Meeting : BaseEntity
    {
        public Guid RequestId { get; set; }
        [ForeignKey(nameof(RequestId))]
        public  Request Request { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        [MaxLength(200)]
        public string Location  { get; set; }
        public string MeetUp { get; set; } = string.Empty;
        [MaxLength(20)]
        public MeetingStatusEnum  Status { get; set; }
    }
}
