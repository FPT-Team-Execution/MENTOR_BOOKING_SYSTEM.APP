namespace MBS.Services.Models.Responses.Major;

public class MajorsResponse
{
    public required IEnumerable<MajorResponse> Majors { get; set; }
}