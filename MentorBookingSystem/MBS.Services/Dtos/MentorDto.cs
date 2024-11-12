namespace MBS.Services.Dtos;

public class MentorDto
{
    public string Id { get; set; } = string.Empty;
    public string? Industry { get; set; } = string.Empty;
    public int ConsumePoint { get; set; } = 0;

    public IEnumerable<MajorDto> Majors { get; set; } = new List<MajorDto>();

    public IEnumerable<DegreeDto> Degrees { get; set; } = new List<DegreeDto>();

    //inheritant
    public string FullName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public DateTime? Birthday { get; set; }

    // IdentityUser Properties
    public string? UserName { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; } = string.Empty;
    public bool EmailConfirmed { get; set; } = false;
    public DateTime? LockoutEnd { get; set; }
    public bool LockoutEnabled { get; set; } = false;
    public string? CreatedBy { get; set; } = string.Empty;
    public DateTime? CreatedOn { get; set; }
    public string? UpdatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedOn { get; set; }
}