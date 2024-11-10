using MBS.BusinessObject.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.BusinessObject.Entities
{
    public class Degree : BaseEntity
    {
        [MaxLength(100), Required] public string Name { get; set; } = default;
        public string? ImageUrl { get; set; } = default;
        [MaxLength(100)] public string? Institution { get; set; } = default;
        [MaxLength(450)] public string MentorId { get; set; }
        [ForeignKey(nameof(MentorId))] public Mentor Mentor { get; set; }
    }
}