using MBS.Services.Constants;
using MBS.Services.Models;
using MBS.Services.Models.Requests.Group;
using MBS.Services.Models.Responses.Group;
using MBS.Services.Models.Responses.Major;
using MBS.Services.Services.Interfaces;
using MBS.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Services.Services.Implements
{
    public class GroupService : IGroupService
    {
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
