namespace MBS.Services.Dtos;

public class ProjectDto
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public string Semester { get; set; } = string.Empty;
    public string? CreatedBy { get; set; } = string.Empty;
    public string MentorId { get; set; } = string.Empty;
    public string MentorName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}