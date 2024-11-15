namespace MBS.Services.Dtos;

public class MeetingDto
{
    public Guid Id { get; set; }
    public string title { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public string MeetUp { get; set; }
    public string Status { get; set; }
}