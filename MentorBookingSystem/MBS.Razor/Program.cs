using MBS.Services.Services;
using MBS.Services.Services.Implements;
using MBS.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddScoped<IAuthService, AuthService>();
// builder.Services.AddAuthentication(options =>
//     {
//         options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//         options.DefaultChallengeScheme = "Google"; // Use the name you specify below
//     })
//     .AddCookie()
//     .AddGoogle(options =>
//     {
//         options.ClientId = "1096581745243-bj51g3cd4rq13codonsoftbk8h7tq4mi.apps.googleusercontent.com";
//         options.ClientSecret = "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";
//         // options.CallbackPath = "/api/auth/signin-google"; // Set the callback path
//     });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
