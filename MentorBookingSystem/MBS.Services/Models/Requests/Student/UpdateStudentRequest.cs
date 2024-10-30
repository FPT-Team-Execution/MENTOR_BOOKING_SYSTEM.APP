using MBS.Services.Constants.Enums;

namespace MBS.Services.Models.Requests.Student;

public class UpdateStudentRequest
{
    public string Id { get; set; } = String.Empty;
    public string FullName { get; set; }  = String.Empty;
    public string? Email { get; set; } = string.Empty;
    public string? University { get; set; } = string.Empty;
    public string MajorId { get; set; } = string.Empty;
    public int WalletPoint { get; set; } = 0;
    public string? AvatarUrl { get; set; } = string.Empty;
    public string Gender { get; set; } = UserGender.Male;
    public DateTime? Birthday { get; set; }
    public bool LockoutEnabled { get; set; } = false;
    public string? UserName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; } = string.Empty;
    public bool EmailConfirmed { get; set; } = false;
    public DateTime? LockoutEnd { get; set; } 
}