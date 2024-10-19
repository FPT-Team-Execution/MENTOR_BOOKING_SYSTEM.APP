using System.ComponentModel.DataAnnotations;

namespace MBS.Services.Models.Requests.Auth;

public class RegisterRequest
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public required string Email { get; set; }

    [Required] public required string Password { get; set; }

    [MaxLength(100)] [Required] public required string FullName { get; set; }

    [MaxLength(10)] [Required] public required string Gender { get; set; }

    public Guid MajorId { get; set; }

    public string? University { get; set; }

    public string? Industry { get; set; }

    public required string Role { get; set; }
}