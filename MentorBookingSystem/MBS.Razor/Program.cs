using MBS.Razor.Extensions;
using MBS.Services.Constants;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
// Register IHttpContextAccessor
builder.Services.AddHttpContextAccessor();
//add Services
builder.Services.AddServiceDependencies();
//add data access dependencies
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddRepositoryDependencies();

builder.Services.AddAuthentication(options =>
    {
        // Set the default authentication scheme to Cookie
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
        // Configure cookie options here
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Use 'None' for non-HTTPS environments

        options.Cookie.Name = "MBS";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.LoginPath = RouteEndpoints.Login;
        options.AccessDeniedPath = RouteEndpoints.Login; 
    });


builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseRouting();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.UseSession();

app.Run();
