using System.ComponentModel.DataAnnotations;

namespace MBS.Services.Models.Requests.Project
{
    public class CreateProjectModel
    {
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        [MaxLength(50)]
        public string Semester { get; set; }
        [MaxLength(450)]
        public string MentorId { get; set; }
    }
}
