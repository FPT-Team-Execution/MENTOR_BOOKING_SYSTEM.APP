using MBS.Services.Constants;
using MBS.Services.Models;
using MBS.Services.Models.Requests.Auth;
using MBS.Services.Models.Responses.Auth;
using MBS.Services.Services.Interfaces;
using MBS.Services.Utils;

namespace MBS.Services.Services.Implements;

public class AuthService : IAuthService
{
    public async Task<BaseModel<LoginResponse, LoginRequest>> LoginAsync(LoginRequest request)
    {
        var result = await WebUtils.PostAsync(ApiEndPoints.LoginUrl, request);
        var response = WebUtils.HandleResponse<BaseModel<LoginResponse, LoginRequest>>(result);
        return response;
        
    }

    public async Task LoginWithGoogleAsync()
    {
        // var result = await WebUtils.GetAsync();
    }
}