namespace MBS.Services.Models.Requests.Degree;

public class GetMentorDegreeRequest
{
    public required string MentorId { get; set; }
    public required int Page { get; set; }
    public required int Size { get; set; }
}