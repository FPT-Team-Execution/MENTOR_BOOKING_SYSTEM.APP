using MBS.Services.Constants;
using MBS.Services.Models.Requests.Auth;
using MBS.Services.Models.Responses.Auth;
using MBS.Services.Services.Interfaces;
using MBS.Services.Utils;

namespace MBS.Services.Services.Implements;

public class AuthService : IAuthService
{
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var result = await WebUtils.PostAsync(ApiEndPoints.LoginUrl, request);
        var response = WebUtils.HandleResponse<LoginResponse>(result);
        return response;
        
    }

    public async Task LoginWithGoogleAsync()
    {
        // var result = await WebUtils.GetAsync();
    }
}