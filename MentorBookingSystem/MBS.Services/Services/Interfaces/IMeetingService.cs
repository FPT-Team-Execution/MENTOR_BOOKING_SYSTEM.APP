using MBS.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBS.DataAccess.Pagination;
using MBS.Services.Dtos;

namespace MBS.Services.Services.Interfaces
{
    public interface IMeetingService
    {
        Task<IResponse> GetMeetingAsync(int page, int size);

        Task<Pagination<MeetingDto>> GetMeetingsByMentorIdPaginationAsync(string mentorId, int page, int size);
        Task<Pagination<MeetingDto>> GetMeetingsPageList(int page, int size);

        
        Task<MeetingDto> GetMeetingByRequestId(string requestId);
        
        Task<MeetingDto> GetMeetingById(string id);
    }
}
