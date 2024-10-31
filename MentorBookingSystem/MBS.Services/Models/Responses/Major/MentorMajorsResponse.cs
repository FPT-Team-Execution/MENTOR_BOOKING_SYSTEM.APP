namespace MBS.Services.Models.Responses.Major;

public class MentorMajorsResponse
{
    public required IEnumerable<MajorResponse> Majors { get; set; }
}