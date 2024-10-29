using MBS.Services.Models;
using MBS.Services.Models.Requests.Group;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Services.Services.Interfaces
{
    public interface IGroupService
    {
        public Task<IResponse> GetGroupsAsync(int page, int size);
        Task<IResponse> CreateNewGroupAsync(CreateNewGroupRequestModel request);

    }
}
