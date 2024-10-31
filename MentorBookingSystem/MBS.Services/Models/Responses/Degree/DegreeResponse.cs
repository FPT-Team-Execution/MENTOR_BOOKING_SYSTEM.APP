namespace MBS.Services.Models.Responses.Degree;

public class DegreeResponse
{
    public required Guid Id { get; set; }
    public string Name { get; set; } = default;
    public string? ImageUrl { get; set; } = default;
    public string? Institution { get; set; } = default;
}