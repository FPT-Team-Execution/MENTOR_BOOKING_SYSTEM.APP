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
using DocumentFormat.OpenXml.Spreadsheet;
using Mapster;
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

        public async Task<Pagination<MeetingDto>> GetMeetingsByMentorIdPaginationAsync(string mentorId, int page, int size)
        {
            var result = await _meetingRepository.GetPageListMeetingByMentorId(mentorId, page, size);
            var itemsResposnes = result.Items.Where(p => p.Request.MentorId == mentorId).Select(p => new MeetingDto
            {
                title = p.Request.Title,
                Description = p.Description,
                Location = p.Location,
                Status = p.Status.ToString(),
                MeetUp = p.MeetUp,
                RequestId = p.RequestId,
                
                
            }).ToList();
            var newPagination = new Pagination<MeetingDto>
            {
                Items = itemsResposnes,
                TotalItems = result.TotalItems,
                TotalPages = result.TotalPages,
                PageSize = size,
                PageIndex = page
            };
            return newPagination;
        }

        public async Task<Pagination<MeetingDto>> GetMeetingsPageList(int page, int size)
        {
            var result = await _meetingRepository.GetPageListMeetings(page, size);
            var itemsResponse = new List<MeetingDto>();
            foreach (var item in result.Items)
            {
                var itemDTO = new MeetingDto();
                itemDTO.title = item.Request.Title;
                itemDTO.Description = item.Description;
                itemDTO.Location = item.Location;
                itemDTO.Status = item.Status.ToString();
                itemDTO.MeetUp = item.MeetUp;
                itemDTO.RequestId = item.RequestId;
            }
        }

        public async Task<MeetingDto> GetMeetingByRequestId(string requestId)
        {
            var meeting = await _meetingRepository.GetMeetingByRequestId(requestId);
            return meeting.Adapt<MeetingDto>();
        }

        public async Task<MeetingDto> GetMeetingById(string id)
        {
            var meeting = await _meetingRepository.GetMeetingId(id);
            return meeting.Adapt<MeetingDto>();
        }
    }
}
