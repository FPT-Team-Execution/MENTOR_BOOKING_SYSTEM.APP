using MBS.Services.Models.Requests.Auth;
using MBS.Services.Models.Responses.Auth;

namespace MBS.Services.Services.Interfaces;

public interface IAuthService
{
    public Task<LoginResponse> LoginAsync(LoginRequest request);
    public Task LoginWithGoogleAsync();
}