using MBS.Services.Models;
using MBS.Services.Models.Requests.Auth;
using MBS.Services.Models.Responses.Auth;

namespace MBS.Services.Services.Interfaces;

public interface IAuthService
{
    public Task<BaseModel<LoginResponse, LoginRequest>> LoginAsync(LoginRequest request);
    public Task LoginWithGoogleAsync();
    
}