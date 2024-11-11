namespace MBS.Services.Dtos;

public class SkillDto
{
    public Guid id { get; set; }
    public string Name { get; set; }
    public string MentorName { get; set; }
    public string MentorEmail { get; set; }
    public string MentorId { get; set; }
}