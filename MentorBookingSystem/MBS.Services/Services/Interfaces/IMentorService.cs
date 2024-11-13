using MBS.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBS.Services.Dtos;
using MBS.Services.Models.Requests.Degree;
using MBS.Services.Models.Requests.Mentor;

using MBS.DataAccess.Pagination;

using MBS.Services.Models.Responses.Mentor;


namespace MBS.Services.Services.Interfaces
{
    public interface IMentorService
    {
        public Task<Pagination<MentorDto>> GetMentorsPaginationAsync(int page, int size);

        public Task<IEnumerable<MentorsResponse>> GetMentorsAsync();


        public Task<IResponse> UpdateMentorAsync(UpdateMentorRequest request);
        public Task<Pagination<DegreeDto>> GetMentorDegrees(string mentorId, int page, int size);
        public Task<MentorDto?> GetMentorById(string id);
        
    }
}