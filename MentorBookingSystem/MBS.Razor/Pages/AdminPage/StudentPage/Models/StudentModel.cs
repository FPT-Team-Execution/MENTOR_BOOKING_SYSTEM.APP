using MBS.Services.Constants.Enums;

namespace MBS.Razor.Pages.AdminPage.StudentPage.Models;

public class StudentModel
{
    public string Id { get; set; } = String.Empty;
    public string FullName { get; set; }  = String.Empty;
    public string? Email { get; set; } = string.Empty;
    public string? University { get; set; } = string.Empty;
    public string MajorName { get; set; } = string.Empty;
    public int WalletPoint { get; set; } = 0;
    public string? AvatarUrl { get; set; } = string.Empty;
    public string Gender { get; set; } = UserGender.Male;
    public DateTime? Birthday { get; set; }
    //false -> active
    public bool LockoutEnabled { get; set; } = false;
  
    //only show on creat view
    public string? UserName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; } = string.Empty;
    public bool EmailConfirmed { get; set; } = false;
    public DateTime? LockoutEnd { get; set; } 
    //No modify
    public string? CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
}
