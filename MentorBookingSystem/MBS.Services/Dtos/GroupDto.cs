namespace MBS.Services.Dtos;

public class GroupDto
{
    public Guid Id {get; set;}
    public Guid ProjectId { get; set;}
    public string StudentId { get; set;}
    public string StudentName { get; set; }
    public Guid PositionId { get; set;}
    public string PositionName { get; set; }
}