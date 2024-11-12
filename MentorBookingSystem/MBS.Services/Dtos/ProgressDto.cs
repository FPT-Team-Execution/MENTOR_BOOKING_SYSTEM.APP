using System.ComponentModel.DataAnnotations;

namespace MBS.Services.Dtos;

public class ProgressDto
{
    public Guid Id { get; set; }
    [MaxLength(100)]
    public string Name  { get; set; } = string.Empty;
    [Required] public bool IsComplete { get; set; } = false;
    public DateTime? CreatedOn { get; set; }
}