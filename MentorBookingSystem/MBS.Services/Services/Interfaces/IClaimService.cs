using System.Security.Claims;

namespace MBS.Services.Services.Interfaces;

public interface IClaimService
{
    Task SignInAsync(List<Claim> claims);
    Task SignOutAsync();
    void AppendCookie(string key, string value);
    Dictionary<string, string> GetClaims();
}