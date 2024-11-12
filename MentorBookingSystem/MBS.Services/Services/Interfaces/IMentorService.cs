using MBS.Services.Models;
using MBS.Services.Dtos;
using MBS.Services.Models.Requests.Degree;
using MBS.Services.Models.Requests.Mentor;

namespace MBS.Services.Services.Interfaces
{
    public interface IMentorService
    {
        public Task<IResponse> GetMentorsAsync(int page, int size);
        public Task<IResponse> UpdateMentorAsync(UpdateMentorRequest request);
        public Task<IResponse> GetMentorDegrees(GetMentorDegreeRequest request);
        public Task<MentorDto?> GetMentorById(string id);

    }
}