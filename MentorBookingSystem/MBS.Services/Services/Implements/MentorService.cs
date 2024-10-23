using MBS.Services.Constants;
using MBS.Services.Models;
using MBS.Services.Models.Responses.Major;
using MBS.Services.Services.Interfaces;
using MBS.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBS.Services.Models.Responses.Mentor;

namespace MBS.Services.Services.Implements
{
    public class MentorService : IMentorService
    {
        public async Task<IResponse> GetMentorsAsync(int page, int size)
        {
            var result = await WebUtils.GetAsync(ApiEndPoints.MentorUrl,
                token: WebUtils.AccessToken,
                queryParams: new Dictionary<string, string?>()
                {
                    { "page", page.ToString() },
                    { "size", size.ToString() }
                });
            var response = WebUtils.HandleResponse<BaseModel<Pagination<MentorResponse>>>(result);
            return response;
        }
    }
}