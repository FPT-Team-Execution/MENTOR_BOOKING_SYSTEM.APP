using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MBS.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MBS.Services.Services.Implements;

public class ClaimService : IClaimService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public ClaimService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task SignInAsync(List<Claim> claims)
    {
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        // Sign in the user
        await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity));
    }
    public async Task SignOutAsync()
    {
        //sign out
        await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
    public void AppendCookie(string key, string value)
    {
        CookieOptions cookieOptions = new CookieOptions
        {
            Expires = DateTimeOffset.Now.AddMinutes(60), // Cookie valid for 60 minutes
            HttpOnly = true, // Prevents access from JavaScript (for security)
            Secure = true,   // Ensure the cookie is only sent over HTTPS
            SameSite = SameSiteMode.Strict // Protects against CSRF attacks
        };
        _httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, cookieOptions);
    }

    public Dictionary<string, string> GetClaims()
    {
        var claimsDictionary = new Dictionary<string, string>();

        // Retrieve the access token from the cookie
        var getAvalable = _httpContextAccessor.HttpContext.Request.Cookies.TryGetValue("MBS", out string accessToken);
        if (!getAvalable)
        {
            return claimsDictionary;
        }
        // Access Token found, now parse it to read claims
        var handler = new JwtSecurityTokenHandler();
        var jwtInfo = handler.ReadJwtToken(accessToken);

        if (jwtInfo != null)
        {
            foreach (var claim in jwtInfo.Claims)
            {
                claimsDictionary[claim.Type] = claim.Value;
            }
        }
        return claimsDictionary;
    }
}