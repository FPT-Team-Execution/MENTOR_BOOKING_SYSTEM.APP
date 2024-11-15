using MBS.BusinessObject.Entities;
using MBS.Externals.Models.Google.GoogleOAuth.Response;
using GoogleTokenResponse = MBS.Services.Models.Responses.Auth.GoogleAuth.GoogleTokenResponse;

namespace MBS.Services.Services.Interfaces;

public interface IAuthService
{
    // public Task<BaseModel<LoginResponse, LoginRequest>> LoginAsync(LoginRequest request);
    public Task<ApplicationUser?> GetUserByEmailAsync(string email);
    public Task<bool> IsPasswordCorrect(ApplicationUser user, string password);
    public Task<IList<string>> GetUserRolesAsync(ApplicationUser user);
    public Task<string> GetUserRoleAsync(ApplicationUser user);
    public string GetGoogleRedirectUrl();
    Task<ApplicationUser?> LoginWithGoogleAsync(GoogleUserInfoResponse gUserInfo);
    // public Task<BaseModel<RegisterResponse, RegisterRequest>> RegisterAsync(RegisterRequest request);
    public Task<bool> CreateUserAsync(ApplicationUser user, string password);
    public Task<bool> AddToRoleAsync(ApplicationUser user, string role);
    Task SendVerifyEmail(ApplicationUser user);
    Task<bool> VerifyEmail(ApplicationUser user, string token);
}