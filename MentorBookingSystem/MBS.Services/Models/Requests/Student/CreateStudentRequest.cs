namespace MBS.Services.Models.Requests.Student;

public class CreateStudentRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; } = "123456789aA!";
    public required string FullName { get; set; }
    public required string Gender { get; set; }
    public Guid MajorId { get; set; }
    public string? University { get; set; }
}