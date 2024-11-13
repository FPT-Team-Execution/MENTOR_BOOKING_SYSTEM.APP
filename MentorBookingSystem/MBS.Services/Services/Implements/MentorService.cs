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
using MBS.DataAccess.Pagination;



namespace MBS.Services.Services.Implements
{
    public class MentorService : IMentorService
    {
        private readonly IMentorRepository _mentorRepository;
        private readonly IDegreeRepository _degreeRepository;
        

        public MentorService(IMentorRepository mentorRepository, IDegreeRepository degreeRepository)
        {
            _mentorRepository = mentorRepository;
            _degreeRepository = degreeRepository;
        }

        public async Task<Pagination<MentorDto>> GetMentorsPaginationAsync(int page, int size)
        {
            var result = await _mentorRepository.GetMentorsPaginationAsync(page, size);
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

        public async Task<Pagination<DegreeDto>> GetMentorDegrees(string mentorId, int page, int size)
        {
            var result = await _degreeRepository.GetDegreesByMentorId(mentorId, page, size);
            return result.Adapt<Pagination<DegreeDto>>();
        }

        public async Task<MentorDto?> GetMentorById(string id)
        {
            var mentor = await _mentorRepository.GetMentorByIdAsync(id);
            return mentor.Adapt<MentorDto>();
        }
        async Task<IEnumerable<MentorsResponse>> IMentorService.GetMentorsAsync()
        {
            var result = await _mentorRepository.GetMentorsAsync();
            var responseList = new List<MentorsResponse>();
            foreach (var mentor in result) {
                var objectParse = new MentorsResponse
                {
                    Id = mentor.UserId,
                    Email = mentor.User.Email,
                    FullName = mentor.User.FullName,
                };
                responseList.Add(objectParse);
            }
            return responseList.Adapt<IEnumerable<MentorsResponse>>();
        }

    }
}