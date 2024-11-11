using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Externals.Services.Interfaces
{
    public interface IClaimExternalService
    {
        string GetUserId();
        Task<AuthenticateResult?> GetAuthenticationAsync(string authenticationScheme);
        string GetClaim(string key);
        string SetCookieValue(string key, string value, DateTime? expireTime);
        string GetCookieValue(string key);
        string GetCookieExpiredTime(string key);
        void DeleteCookie(string key);
    }
}
