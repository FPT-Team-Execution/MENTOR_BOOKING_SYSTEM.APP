using MBS.Externals.Services.Implements;
using MBS.Externals.Services.Interfaces;
using MBS.Services.Services.Implements;
using MBS.Services.Services.Interfaces;

namespace MBS.Razor.Extensions;

public static class ServiceDependency
{
    public static void AddServiceDependencies(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IMajorService, MajorService>();
        services.AddScoped<IClaimService, ClaimService>();
        services.AddScoped<IClaimExternalService, ClaimExternalService>();
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<IMentorService, MentorService>();
        services.AddScoped<IGroupService, GroupService>();
        services.AddScoped<IPositionService, PositionService>();
        services.AddScoped<ISkillService, SkillService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IMeetingService, MeetingService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ITemplateService, TemplateService>();
    }
}