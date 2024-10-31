namespace MBS.Services.Models.Requests.Major;

public class GetMentorMajorsRequest
{
    public required string MentorId { get; set; }
    public required int Page { get; set; }
    public required int Size { get; set; }
}