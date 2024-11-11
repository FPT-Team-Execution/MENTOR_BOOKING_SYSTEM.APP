namespace MBS.Services.Models.Responses.Major;

public class MajorsResponse
{
    public required IEnumerable<MajorResponseDto> Majors { get; set; }
}