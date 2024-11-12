using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MBS.BusinessObject.Entities;

namespace MBS.Services.Dtos;

public class ProcessDto
{
    [MaxLength(100)]
    public string Name  { get; set; } = string.Empty;
    [Required] public bool IsComplete { get; set; } = false;
    public DateTime? CreatedOn { get; set; }
}