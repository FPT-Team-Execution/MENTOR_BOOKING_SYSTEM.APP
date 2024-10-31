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
using MBS.Services.Models.Requests.Degree;
using MBS.Services.Models.Requests.Mentor;
using MBS.Services.Models.Responses.Degree;
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

        public async Task<IResponse> UpdateMentorAsync(UpdateMentorRequest request)
        {
            var token = WebUtils.AccessToken;
            var result = await WebUtils.PutAsync
            (
                ApiEndPoints.MentorUpdateUrl,
                data: request,
                headers: new Dictionary<string, string>
                {
                    { "Accept-Charset", "utf-8" },
                    { "Authorization", $"Bearer {token}" }
                },
                token: token
            );
            var response = WebUtils.HandleResponse<BaseModel<UpdateMentorResponse>>(result);
            return response;
        }

        public async Task<IResponse> GetMentorDegrees(GetMentorDegreeRequest request)
        {
            var token = WebUtils.AccessToken;
            var result = await WebUtils.GetAsync
            (
                ApiEndPoints.MentorDegreeDisplayUrl,
                queryParams: new Dictionary<string, string?>()
                {
                    { "mentorId", request.MentorId },
                    { "page", request.Page.ToString() },
                    { "size", request.Size.ToString() }
                },
                headers: new Dictionary<string, string>
                {
                    { "Accept-Charset", "utf-8" },
                    { "Authorization", $"Bearer {token}" }
                },
                token: token
            );
            var response = WebUtils.HandleResponse<BaseModel<Pagination<DegreesResponse>>>(result);
            return response;
        }
    }
}