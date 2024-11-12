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
using Mapster;
using MBS.Repositories.Interfaces;
using MBS.Services.Dtos;
using MBS.Services.Models.Requests.Degree;
using MBS.Services.Models.Requests.Mentor;
using MBS.Services.Models.Responses.Degree;
using MBS.Services.Models.Responses.Mentor;

namespace MBS.Services.Services.Implements
{
    public class MentorService : IMentorService
    {
        private readonly IMentorRepository _mentorRepository;

        public MentorService(IMentorRepository mentorRepository)
        {
            _mentorRepository = mentorRepository;
        }

        // public async Task<IResponse> GetMentorsAsync(int page, int size)
        // {
        //     var result = await WebUtils.GetAsync(ApiEndPoints.MentorUrl,
        //         token: WebUtils.AccessToken,
        //         queryParams: new Dictionary<string, string?>()
        //         {
        //             { "page", page.ToString() },
        //             { "size", size.ToString() }
        //         });
        //     var response = WebUtils.HandleResponse<BaseModel<Pagination<MentorResponse>>>(result);
        //     return response;
        // }

        public async Task<Pagination<MentorDto>> GetMentorsAsync(int page, int size)
        {
            var result = await _mentorRepository.GetPagedListAsync(page, size);
            return result.Adapt<Pagination<MentorDto>>();
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
                ApiEndPoints.MentorDegreeUrl(request.MentorId),
                queryParams: new Dictionary<string, string?>()
                {
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

            var response = WebUtils.HandleResponse<BaseModel<Pagination<DegreeResponse>>>(result);
            return response;
        }
    }
}