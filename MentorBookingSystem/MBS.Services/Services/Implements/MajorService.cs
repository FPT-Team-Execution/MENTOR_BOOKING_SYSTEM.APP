using Mapster;
using MBS.Repositories.Interfaces;
using MBS.Services.Constants;
using MBS.Services.Dtos;
using MBS.Services.Models;
using MBS.Services.Models.Requests.Auth;
using MBS.Services.Models.Requests.Major;
using MBS.Services.Models.Responses;
using MBS.Services.Models.Responses.Group;
using MBS.Services.Models.Responses.Major;
using MBS.Services.Services.Interfaces;
using MBS.Services.Utils;

namespace MBS.Services.Services.Implements;

public class MajorService : IMajorService
{
    private readonly IMajorRepository _majorRepository;

    public MajorService(IMajorRepository majorRepository)
    {
        _majorRepository = majorRepository;
    }

    public async Task<IResponse> GetMentorMajorsAsync(GetMentorMajorsRequest request)
    {
        var token = WebUtils.AccessToken;
        var result = await WebUtils.GetAsync
        (
            ApiEndPoints.MentorMajorUrl(request.MentorId),
            queryParams: new Dictionary<string, string?>()
            {
                { "page", request.Page.ToString() },
                { "size", request.Size.ToString() }
            },
            headers: new Dictionary<string, string>
            {
                { "Accept-Charset", "utf-8" },
                { "Authorization", $"Bearer {token}" }
            },
            token: token
        );
        var response = WebUtils.HandleResponse<BaseModel<Pagination<MajorResponseDto>>>(result);
        return response;
    }

    public async Task<IResponse> CreateNewMajorAsync(CreateNewMajorRequestModel request)
    {
        var result = await WebUtils.PostAsync(
            ApiEndPoints.MajorUrl,
            request,
            token: WebUtils.AccessToken
        );
        var response = WebUtils.HandleResponse<BaseModel<GroupResponse>>(result);
        return response;
    }

    public async Task<IEnumerable<MajorDto>> GetAllMajors()
    {
        var majors = await _majorRepository.GetAllAsync();
        return majors.Adapt<IEnumerable<MajorDto>>();
    }

    public async Task<IResponse> GetMajorsAsync(int page, int size)
    {
        var result = await WebUtils.GetAsync
        (
            ApiEndPoints.MajorUrl,
            queryParams: new Dictionary<string, string?>()
            {
                { "page", page.ToString() },
                { "size", size.ToString() }
            },
            token: WebUtils.AccessToken
        );
        var response = WebUtils.HandleResponse<BaseModel<Pagination<MajorResponseDto>>>(result);
        return response;
    }
}