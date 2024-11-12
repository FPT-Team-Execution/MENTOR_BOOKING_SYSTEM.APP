using MBS.DataAccess;
using MBS.DataAccess.DAO.Implements;
using MBS.DataAccess.DAO.Interfaces;
using MBS.Repositories.Implements;
using MBS.Repositories.Interfaces;
using MBS.Services.Services.Implements;
using MBS.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MBS.Razor.Extensions;

public static class DataAccessDependency
{
    public static void AddRepositoryDependencies(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseDAO<>), typeof(BaseDAO<>));
        services.AddScoped<IMajorRepository, MajorRepository>();
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IGroupRepository, GroupRepository>();
        services.AddScoped<IRequestRepository, RequestRepository>();
        services.AddScoped<IMentorRepository, MentorRepository>();

    }

    public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MBSContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("MBS"),
                opt => opt.MigrationsAssembly(typeof(MBSContext).Assembly.FullName)));
    }
}