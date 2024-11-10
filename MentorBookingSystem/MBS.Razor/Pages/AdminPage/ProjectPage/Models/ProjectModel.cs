namespace MBS.Razor.Pages.AdminPage.ProjectPage.Models
{
    public class ProjectModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Semester { get; set; }
        public string? CreatedBy { get; set; }
        public string MentorId { get; set; }
        public string Status
        {
            get; set;
        }
    }
}
