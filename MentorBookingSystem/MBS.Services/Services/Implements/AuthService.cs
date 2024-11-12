using MBS.BusinessObject.Entities;
using MBS.Services.Constants;
using MBS.Services.Models;
using MBS.Services.Models.Requests.Auth;
using MBS.Services.Models.Responses;
using MBS.Services.Models.Responses.Auth;
using MBS.Services.Models.Responses.Auth.GoogleAuth;
using MBS.Services.Services.Interfaces;
using MBS.Services.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace MBS.Services.Services.Implements;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthService(IConfiguration configuration, UserManager<ApplicationUser> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }
    // public async Task<BaseModel<LoginResponse, LoginRequest>> LoginAsync(LoginRequest request)
    // {
    //     var result = await WebUtils.PostAsync(ApiEndPoints.LoginUrl, request);
    //     var response = WebUtils.HandleResponse<BaseModel<LoginResponse, LoginRequest>>(result);
    //     return response;
    // }

    public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<bool> IsPasswordCorrect(ApplicationUser user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
    {
        return await _userManager.GetRolesAsync(user);
    }

    public async Task<string> GetUserRoleAsync(ApplicationUser user)
    {
        var result = await _userManager.GetRolesAsync(user);
        return result.ToList().First();
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
            { "callbackUri", googleAuthSettings["RedirectUrl"]! },
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

    public async Task<bool> CreateUserAsync(ApplicationUser user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);
        return result.Succeeded;
    }

    public async Task<bool> AddToRoleAsync(ApplicationUser user, string role)
    {
        var result = await _userManager.AddToRoleAsync(user, role);
        return result.Succeeded;
    }

    // public async Task<BaseModel<RegisterResponse, RegisterRequest>> RegisterAsync(RegisterRequest request)
    // {
    //     var result = await WebUtils.PostAsync(ApiEndPoints.RegisterUrl, request);
    //     var response = WebUtils.HandleResponse<BaseModel<RegisterResponse, RegisterRequest>>(result);
    //     return response;
    // }
}