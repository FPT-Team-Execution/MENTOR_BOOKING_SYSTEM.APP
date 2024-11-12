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

namespace MBS.Services.Services.Interfaces
{
    public interface IMentorService
    {
        public Task<Pagination<MentorDto>> GetMentorsAsync(int page, int size);
        public Task<IResponse> UpdateMentorAsync(UpdateMentorRequest request);
        public Task<Pagination<DegreeDto>> GetMentorDegrees(string mentorId, int page, int size);
        public Task<MentorDto?> GetMentorById(string id);
        
    }
}