namespace MBS.Services.Dtos;

public class GroupDto
{
    public Guid Id { get; set; } = Guid.Empty;
    public Guid ProjectId { get; set;} = Guid.Empty;
    public string StudentId { get; set;} = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public Guid PositionId { get; set;} = Guid.Empty;
    public string PositionName { get; set; } = string.Empty;
}