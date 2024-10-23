using System.Security.Claims;
using MBS.Services.Constants;
using MBS.Services.Models.Responses.Student;
using MBS.Services.Services.Implements;
using MBS.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
// Register IHttpContextAccessor
builder.Services.AddHttpContextAccessor();
//add Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMajorService, MajorService>();
builder.Services.AddScoped<IClaimService, ClaimService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IMentorService, MentorService>();

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

app.UseRouting();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.UseSession();

app.Run();
