using MBS.Services.Models;
using MBS.Services.Models.Requests.Group;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBS.Services.Dtos;

namespace MBS.Services.Services.Interfaces
{
    public interface IGroupService
    {
        public Task<IEnumerable<GroupDto>> GetGroupsByStudentIdAsync(string userId, string? projectStatus = null);
        public Task<IEnumerable<GroupDto>> GetGroupsByProjectIdAsync(Guid projectId);
        public Task<IResponse> GetGroupsAsync(int page, int size);
        Task<IResponse> CreateNewGroupAsync(CreateNewGroupRequestModel request);
    }
}