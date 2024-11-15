using System.Transactions;
using MBS.BusinessObject.Entities;
using MBS.BusinessObject.Enums;
using MBS.Externals.Models.Email;
using MBS.Externals.Models.Google.GoogleOAuth.Response;
using MBS.Externals.Services.Interfaces;
using MBS.Externals.Templates;
using MBS.Repositories.Interfaces;
using MBS.Services.Models;
using MBS.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using GoogleTokenResponse = MBS.Services.Models.Responses.Auth.GoogleAuth.GoogleTokenResponse;

namespace MBS.Services.Services.Implements;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITemplateService _templateService;
    private readonly IEmailService _emailService;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IMentorRepository _mentorRepository;

    public AuthService(
        IConfiguration configuration,
        UserManager<ApplicationUser> userManager,
        ITemplateService templateService,
        IEmailService emailService,
        SignInManager<ApplicationUser> signInManager,
        IMentorRepository mentorRepository)
    {
        _configuration = configuration;
        _userManager = userManager;
        _templateService = templateService;
        _emailService = emailService;
        _signInManager = signInManager;
        _mentorRepository = mentorRepository;
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
        var meetingScope = Uri.EscapeDataString(googleAuthSettings["Scopes:Meeting"]!);

        #endregion

        var scope = $"{calendarScope} {profileScope} {emailScope} {meetingScope}";
        var responseType = googleAuthSettings["ResponseType"];
        //* prompt=consent is optional based on business
        //* state is optional
        var googleAuthUrl =
            $"{url}?redirect_uri={redirectUrl}&response_type={responseType}&client_id={clientId}&scope={scope}&access_type=offline";
        return googleAuthUrl;
    }

    public async Task<ApplicationUser?> LoginWithGoogleAsync(GoogleUserInfoResponse gUserInfo)
    {
        //Login Or Sign Up for account
        var result = await LoginOrSignUpExternal(gUserInfo);
        return result;
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

    public async Task SendVerifyEmail(ApplicationUser user)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        var emailTemplate = await _templateService.GetTemplateAsync(TemplateConstants.ConfirmationEmail);

        var emailBody = _templateService.ReplaceInTemplate(emailTemplate,
            new Dictionary<string, string> { { "{Email}", user.Email! }, { "{Token}", token } });

        await _emailService.SendEmailAsync(EmailMessage.Create(user.Email!, emailBody, "[MBS]Confirm your email"));
    }

    public async Task<bool> VerifyEmail(ApplicationUser user, string token)
    {
        var result = await _userManager.ConfirmEmailAsync(user, token);
        return result.Succeeded;
    }
    // public async Task<BaseModel<RegisterResponse, RegisterRequest>> RegisterAsync(RegisterRequest request)
    // {
    //     var result = await WebUtils.PostAsync(ApiEndPoints.RegisterUrl, request);
    //     var response = WebUtils.HandleResponse<BaseModel<RegisterResponse, RegisterRequest>>(result);
    //     return response;
    // }

    private async Task<ApplicationUser?> LoginOrSignUpExternal(GoogleUserInfoResponse gUserInfo)
    {
        try
        {
            //Try Sign in by external information
            var tryExternalLogin =
                await _signInManager.ExternalLoginSignInAsync("Google", gUserInfo.sub, true);
            //if success, get info user and return result
            if (tryExternalLogin.Succeeded)
            {
                var user = await _userManager.FindByLoginAsync("Google", gUserInfo.sub);
                if (user == null)
                    return null;

                return user;
            }

            //if user is new -> create new account
            var userCreate = new ApplicationUser
            {
                Email = gUserInfo.email,
                UserName = gUserInfo.email,
                FullName = gUserInfo.name,
                AvatarUrl = gUserInfo.picture,
                EmailConfirmed = gUserInfo.email_verified,
                //TODO: get more info from email
            };
            
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                //create user, add role,add external login 
                //* create user
                var createResult = await _userManager.CreateAsync(userCreate);
                if (!createResult.Succeeded)
                    return null;

                //create mentor
                var mentorCreate = new Mentor()
                {
                    UserId = userCreate.Id,
                };
                var addMentorResult = await _mentorRepository.CreateAsync(mentorCreate);
                if (!addMentorResult)
                {
                    return null;
                }

                //*add role
                var user = await _userManager.FindByEmailAsync(userCreate.Email);
                await _userManager.AddToRoleAsync(user, UserRoleEnum.Mentor.ToString());
                //*Add external login
                var userLoginInfo = new UserLoginInfo(providerKey: gUserInfo.sub, loginProvider: "Google",
                    displayName: "Google");
                var addResult = await _userManager.AddLoginAsync(userCreate, userLoginInfo);
                if (addResult.Succeeded)
                {
                    transactionScope.Complete();
                    return user;
                }
            }

            return null;
        }
        catch (Exception e)
        {
            return null;
        }
    }
}