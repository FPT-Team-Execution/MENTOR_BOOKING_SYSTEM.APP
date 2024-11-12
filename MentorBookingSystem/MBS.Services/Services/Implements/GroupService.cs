using MBS.Services.Constants;
using MBS.Services.Models;
using MBS.Services.Models.Requests.Group;
using MBS.Services.Models.Responses.Group;
using MBS.Services.Services.Interfaces;
using MBS.Services.Utils;
using Mapster;
using MBS.Repositories.Interfaces;
using MBS.Services.Dtos;

namespace MBS.Services.Services.Implements
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        public GroupService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<IEnumerable<GroupDto>> GetGroupsByStudentIdAsync(string userId, string? projectStatus = null)
        {
            var groups = await  _groupRepository.GetGroupsByStudentId(userId, projectStatus);
            return groups.Adapt<IEnumerable<GroupDto>>();
        }

        public async Task<IEnumerable<GroupDto>> GetGroupsByProjectIdAsync(Guid projectId)
        {
            var groups = await  _groupRepository.GetGroupByProjectIdAsync(projectId);
            return groups.Adapt<IEnumerable<GroupDto>>();
        }

        public async Task<IResponse> GetGroupsAsync(int page, int size)
        {
            var result = await WebUtils.GetAsync
            (
               
                ApiEndPoints.GroupUrl,
                queryParams: new Dictionary<string, string?>()
                {
                { "page", page.ToString() },
                { "size", size.ToString() }
                },
                token: WebUtils.AccessToken
            );
            var response = WebUtils.HandleResponse<BaseModel<Pagination<GroupResponse>>>(result);
            return response;
        }

        public async Task<IResponse> CreateNewGroupAsync(CreateNewGroupRequestModel request)
        {
            var result = await WebUtils.PostAsync(
                ApiEndPoints.GroupUrl,
                request,
                token: WebUtils.AccessToken
            );

            var response = WebUtils.HandleResponse<BaseModel<GroupResponse>>(result);
            return response;
        }

    }
}
