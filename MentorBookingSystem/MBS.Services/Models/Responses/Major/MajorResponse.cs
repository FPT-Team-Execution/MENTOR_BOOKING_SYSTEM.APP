using System.ComponentModel.DataAnnotations;

namespace MBS.Services.Models.Responses.Major;

public class MajorResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid ParentId { get; set; }
    public string MajorParentName { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public string Status { get; set; }
}