namespace MBS.Services.Models.Responses.Degree;

public class DegreesResponse
{
    public required IEnumerable<DegreeResponse> Degrees { get; set; }
}