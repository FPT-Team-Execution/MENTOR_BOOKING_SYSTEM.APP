using MBS.BusinessObject.Entities;
using MBS.Services.Models;
using MBS.Services.Models.Requests.Auth;
using MBS.Services.Models.Responses;
using MBS.Services.Models.Responses.Auth;
using MBS.Services.Models.Responses.Auth.GoogleAuth;

namespace MBS.Services.Services.Interfaces;

public interface IAuthService
{
    // public Task<BaseModel<LoginResponse, LoginRequest>> LoginAsync(LoginRequest request);
    public Task<ApplicationUser?> GetUserByEmailAsync(string email);
    public Task<bool> IsPasswordCorrect(ApplicationUser user, string password);
    public Task<IList<string>> GetUserRolesAsync(ApplicationUser user);
    public Task<string> GetUserRoleAsync(ApplicationUser user);
    public string GetGoogleRedirectUrl();
    public Task<BaseModel<GoogleSignInResponse>> LoginWithGoogleAsync(string code);

    // public Task<BaseModel<RegisterResponse, RegisterRequest>> RegisterAsync(RegisterRequest request);
    public Task<bool> CreateUserAsync(ApplicationUser user, string password);
    public Task<bool> AddToRoleAsync(ApplicationUser user, string role);
    Task SendVerifyEmail(ApplicationUser user);
    Task<bool> VerifyEmail(ApplicationUser user, string token);
}