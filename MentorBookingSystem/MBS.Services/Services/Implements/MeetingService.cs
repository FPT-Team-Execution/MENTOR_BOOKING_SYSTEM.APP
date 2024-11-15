using MBS.DataAccess.Pagination;
using MBS.Services.Constants;
using MBS.Services.Models;
using MBS.Services.Models.Responses.Meeting;
using MBS.Services.Models.Responses.Project;
using MBS.Services.Services.Interfaces;
using MBS.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBS.Repositories.Interfaces;
using MBS.Services.Dtos;

namespace MBS.Services.Services.Implements
{
    public class MeetingService : IMeetingService
    {
        private readonly IMeetingRepository _meetingRepository;

        public MeetingService(IMeetingRepository meetingRepository)
        {
            _meetingRepository = meetingRepository;
        }
        public async Task<IResponse> GetMeetingAsync(int page, int size)
        {
            var url = ApiEndPoints.Meeting.Replace("{page}", page.ToString()).Replace("{size}", size.ToString());
            var result = await WebUtils.GetAsync(url,token: WebUtils.AccessToken);
            var response = WebUtils.HandleResponse<BaseModel<Pagination<MeetingResponse>>>(result);
            return response;
        }

        public async Task<Pagination<MeetingDto>> GetMeetingsPaginationAsync(string meetingId, int page, int size)
        {
            var result = await _meetingRepository.
        }
        
        public async Task<Pagination<MajorDto>> GetMentorMajorsAsync(string mentorId, int page, int size)
        {
            var result = await _meetingRepository.Ge(mentorId, page, size);
            return result.Adapt<Pagination<MajorDto>>();
        }
    }
}
