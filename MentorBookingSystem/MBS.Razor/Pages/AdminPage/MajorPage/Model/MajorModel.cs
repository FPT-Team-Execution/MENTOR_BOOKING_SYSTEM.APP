using System.ComponentModel.DataAnnotations;

namespace MBS.Razor.Pages.AdminPage.MajorPage.Model
{
    public class MajorModel
    {
        public Guid Id { get; set; }
        public string MajorName { get; set; }
        public Guid ParentId { get; set; }
        public string ParentName { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string Status { get; set; }
    }
}
