using MBS.BusinessObject.Entities;
using MBS.BusinessObject.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Repositories.Interfaces
{
    public interface IFeedbackRepository : IBaseRepository<Feedback>
    {
        public Task<Pagination<Feedback>> GetFeedBacksByMeetingId(Guid meetingId, int page, int size, string sortBy);
        public Task<Pagination<Feedback>> GetMeetingFeedBacksByUserId(Guid meetingId, string userId, int page, int size, string sortBy);
        public Task<Pagination<Feedback>> GetFeedBacksByMentorId(string mentorId, int page, int size);
        public Task<Pagination<Feedback>> GetAllFeedbacks(int page, int size);





    }
}
