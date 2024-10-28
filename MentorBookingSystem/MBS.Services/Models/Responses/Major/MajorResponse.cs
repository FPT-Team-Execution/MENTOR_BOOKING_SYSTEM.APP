using System.ComponentModel.DataAnnotations;

namespace MBS.Services.Models.Responses.Major;

public class MajorResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
   public string? ParentName { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }
    //public string MajorName { get; set; }
    //public Guid ParentId { get; set; }
}