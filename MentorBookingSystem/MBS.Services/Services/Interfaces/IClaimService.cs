using System.Security.Claims;

namespace MBS.Services.Services.Interfaces;

public interface IClaimService
{
    Task SignInAsync(List<Claim> claims);
    Task SignOutAsync();
    void AppendCookie(string key, string value);
    // Dictionary<string, string> GetClaims();
    
    string GetClaim(string key);
    string SetCookieValue(string key, string value, DateTime? expireTime);
    string GetCookieValue(string key);
    string GetCookieExpiredTime(string key);
    void DeleteCookie(string key);
}