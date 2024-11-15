using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MBS.BusinessObject.Entities;
using MBS.BusinessObject.Enums;

namespace MBS.Services.Dtos;

public class RequestDto
{
    public Guid Id { get; set; } = Guid.Empty;
    [MaxLength(100)]
    [Required]
    public string Title { get; set; } = string.Empty;
    [Required]
    public DateTime Start { get; set; } 
    [Required]
    public DateTime End { get; set; }
    public  string MentorId { get; set; } = string.Empty;
    public string MentorName { get; set; } = string.Empty;
    [MaxLength(450)]
    public string CreaterId { get; set; } = string.Empty;
    public string CreaterName { get; set; } = string.Empty;
    [MaxLength(20)]
    public string Status { get; set; } = string.Empty;
    public DateTime? CreatedOn { get; set; }
    
}