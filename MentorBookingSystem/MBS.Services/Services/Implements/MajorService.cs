using MBS.Services.Constants;
using MBS.Services.Models;
using MBS.Services.Models.Requests.Auth;
using MBS.Services.Models.Responses;
using MBS.Services.Models.Responses.Major;
using MBS.Services.Services.Interfaces;
using MBS.Services.Utils;

namespace MBS.Services.Services.Implements;

public class MajorService : IMajorService
{
    public async Task<IResponse> GetMajorsAsync(int page, int size)
    {
        var result = await WebUtils.GetAsync
        (
            ApiEndPoints.MajorUrl,
            queryParams: new Dictionary<string, string?>()
            {
                { "page", page.ToString() },
                { "size", size.ToString() }
            }
        );
        var response = WebUtils.HandleResponse<BaseModel<Pagination<MajorResponse>>>(result);
        return response;
    }
}