using MBS.Services.Constants;
using MBS.Services.Models;
using MBS.Services.Models.Requests.Auth;
using MBS.Services.Models.Responses;
using MBS.Services.Models.Responses.Auth;
using MBS.Services.Models.Responses.Auth.GoogleAuth;
using MBS.Services.Services.Interfaces;
using MBS.Services.Utils;
using Microsoft.Extensions.Configuration;

namespace MBS.Services.Services.Implements;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    public AuthService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<BaseModel<LoginResponse, LoginRequest>> LoginAsync(LoginRequest request)
    {
        var result = await WebUtils.PostAsync(ApiEndPoints.LoginUrl, request);
        var response = WebUtils.HandleResponse<BaseModel<LoginResponse, LoginRequest>>(result);
        return response;
    }

    public string GetGoogleRedirectUrl()
    {
        var googleAuthSettings = _configuration.GetSection("Google:Auth");
        var url = googleAuthSettings["Url"];
        var clientId = googleAuthSettings["ClientId"];
        var redirectUrl = googleAuthSettings["RedirectUrl"];
        #region Scopes
        var calendarScope = Uri.EscapeDataString(googleAuthSettings["Scopes:Calendar"]!);
        var profileScope = Uri.EscapeDataString(googleAuthSettings["Scopes:Profile"]!);
        var emailScope = Uri.EscapeDataString(googleAuthSettings["Scopes:Email"]!);
        #endregion
        var scope = $"{calendarScope} {profileScope} {emailScope}";
        var responseType = googleAuthSettings["ResponseType"];
        //* prompt=consent is optional based on business
        //* state is optional
        var googleAuthUrl =
            $"{url}?redirect_uri={redirectUrl}&response_type={responseType}&client_id={clientId}&scope={scope}&access_type=offline";
        return googleAuthUrl;
    }

    public async Task<BaseModel<GoogleSignInResponse>> LoginWithGoogleAsync(string code)
    {
        var googleAuthSettings = _configuration.GetSection("Google:Auth");
        var queryParams = new Dictionary<string, string>
        {
            { "code", code },
            { "callbackUri", googleAuthSettings["RedirectUrl"]!},
        };
        var headers = new Dictionary<string, string>
        {
            { "Accept-Charset", "utf-8" },
        };
        var result = await WebUtils.GetAsync(
            url: ApiEndPoints.LoginWithGoogleUrl,
            headers: headers,
            queryParams: queryParams!
            );
        var response = WebUtils.HandleResponse<BaseModel<GoogleSignInResponse>>(result);
        return response;
    }

    public async Task<BaseModel<RegisterResponse, RegisterRequest>> RegisterAsync(RegisterRequest request)
    {
        var result = await WebUtils.PostAsync(ApiEndPoints.RegisterUrl, request);
        var response = WebUtils.HandleResponse<BaseModel<RegisterResponse, RegisterRequest>>(result);
        return response;
    }
}