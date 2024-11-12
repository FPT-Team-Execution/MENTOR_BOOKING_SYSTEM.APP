using MBS.DataAccess.Pagination;
using MBS.Services.Constants;
using MBS.Services.Models;
using MBS.Services.Models.Requests.Skill;
using MBS.Services.Models.Responses.Position;
using MBS.Services.Models.Responses.Skill;
using MBS.Services.Services.Interfaces;
using MBS.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Services.Services.Implements
{
    public class SkillService : ISkillService
    {
        public async Task<IResponse> CreateNewSkillAsync(CreateNewSkillRequestDTO request)
        {
            var result = await WebUtils.PostAsync(
   ApiEndPoints.SkillUrl,
   request,
   token: WebUtils.AccessToken
);

            var response = WebUtils.HandleResponse<BaseModel<SkillResponseDTO>>(result);
            return response;
        }

        public async Task<IResponse> GetSkillsAsync(int page, int size)
        {
            var result = await WebUtils.GetAsync
(

    ApiEndPoints.SkillUrl,
    queryParams: new Dictionary<string, string?>()
    {
    { "page", page.ToString() },
    { "size", size.ToString() }
    },
    token: WebUtils.AccessToken
);
            var response = WebUtils.HandleResponse<BaseModel<Pagination<SkillResponseDTO>>>(result);
            return response;
        }
    }
}
