using MBS.DataAccess.Pagination;
using MBS.Services.Constants;
using MBS.Services.Models;
using MBS.Services.Models.Requests.Position;
using MBS.Services.Models.Responses.Group;
using MBS.Services.Models.Responses.Position;
using MBS.Services.Services.Interfaces;
using MBS.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Services.Services.Implements
{
    public class PositionService : IPositionService
    {
        public async Task<IResponse> CreateNewPositionAsync(CreateNewPositionRequestModel request)
        {
            var result = await WebUtils.PostAsync(
               ApiEndPoints.PositionsUrl,
               request,
               token: WebUtils.AccessToken
           );

            var response = WebUtils.HandleResponse<BaseModel<PositionResponseDTO>>(result);
            return response;
        }

        public async Task<IResponse> GetPositionsAsync(int page, int size)
        {
            var result = await WebUtils.GetAsync
            (

                ApiEndPoints.PositionsUrl,
                queryParams: new Dictionary<string, string?>()
                {
                { "page", page.ToString() },
                { "size", size.ToString() }
                },
                token: WebUtils.AccessToken
            );
            var response = WebUtils.HandleResponse<BaseModel<Pagination<PositionResponseDTO>>>(result);
            return response;
        }
    }
}
