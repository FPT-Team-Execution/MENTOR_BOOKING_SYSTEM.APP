using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MBS.Externals.Services.Interfaces;

namespace MBS.Externals.Services.Implements
{
    public class ClaimExternalService : IClaimExternalService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimExternalService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

        }

        public string GetUserId()
        {
            return GetClaim(ClaimTypes.NameIdentifier);
        }

        public async Task<AuthenticateResult?> GetAuthenticationAsync(string authenticationScheme)
        {
            return await _httpContextAccessor.HttpContext.AuthenticateAsync();
        }

        public string GetClaim(string key)
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(key)?.Value;
        }
        #region Cookie 
        public string SetCookieValue(string key, string value, DateTime? expireTime)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
                option.Expires = expireTime;
            else
                option.Expires = DateTime.Now.AddDays(7); //default expired time is 7 days

            _httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, option);

            if (expireTime != null)
            {
                //Save expired time in other cookie key
                _httpContextAccessor.HttpContext.Response.Cookies.Append($"{key}_expires", expireTime.ToString(), option);
            }
            return key;
        }
        public string GetCookieValue(string key)
        {
            return _httpContextAccessor.HttpContext.Request.Cookies[key];
        }
        public string GetCookieExpiredTime(string key)
        {
            return _httpContextAccessor.HttpContext.Request.Cookies[$"{key}_expires"];
        }
        public void DeleteCookie(string key)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(key);
        }
        #endregion

    }
}
