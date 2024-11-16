using MBS.Services.Constants;
using MBS.Services.Models;
using MBS.Services.Models.Requests.Group;
using MBS.Services.Models.Responses.Group;
using MBS.Services.Services.Interfaces;
using MBS.Services.Utils;
using Mapster;
using MBS.BusinessObject.Entities;
using MBS.Repositories.Interfaces;
using MBS.Services.Dtos;
using MBS.DataAccess.Pagination;

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
            return groups.Adapt<List<GroupDto>>();
        }

        public async Task<IEnumerable<GroupDto>> GetGroupsByProjectIdAsync(Guid projectId)
        {
            var groups = await  _groupRepository.GetGroupByProjectIdAsync(projectId);
            var result =  groups.Adapt<List<GroupDto>>();
            return result;
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

        public async Task<bool> CreateNewGroupAsync(CreateNewGroupRequestModel request)
        {
            try
            {
                Group group = new Group()
                {
                    ProjectId = request.ProjectId,
                    StudentId = request.StudentId,
                    PositionId = request.PositionId,
                };
                var result =  await _groupRepository.Create(group);
                return result;
            }
            catch (Exception e)
            {
                return false;
            }
            
        }
    }
}
