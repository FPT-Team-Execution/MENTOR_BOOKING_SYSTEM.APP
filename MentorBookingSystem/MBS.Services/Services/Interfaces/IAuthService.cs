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
    public string GetGoogleRedirectUrl();
    public Task<BaseModel<GoogleSignInResponse>> LoginWithGoogleAsync(string code);

    public Task<BaseModel<RegisterResponse, RegisterRequest>> RegisterAsync(RegisterRequest request);
}