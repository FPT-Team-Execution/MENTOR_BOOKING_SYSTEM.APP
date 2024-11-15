using Mapster;
using MBS.BusinessObject.Enums;
using MBS.DataAccess.Pagination;
using MBS.Repositories.Interfaces;
using MBS.Services.Dtos;
using MBS.Services.Models.Responses;
using MBS.Services.Models.Responses.Requests;
using MBS.Services.Services.Interfaces;

namespace MBS.Services.Services.Implements;

public class RequestService : IRequestService
{
    private readonly IRequestRepository _requestRepository;

    public RequestService(IRequestRepository requestRepository)
    {
        _requestRepository = requestRepository; 
    }
    public async Task<Pagination<RequestDto>> GetRequestsByProjectIdPaginationAsync(Guid projectId, int pageNumber, int pageSize,  string sortOrder = "desc", string? projectStatus = null)
    {
        var request = await _requestRepository.GetRequestByProjectIdPaginationAsync(projectId, pageNumber, pageSize, sortOrder, projectStatus);
        return request.Adapt<Pagination<RequestDto>>();
    }

    public async Task<Pagination<RequestResponse>> GetAllRequestByMentorId(string mentorId, int page, int size)
    {
        var result = await _requestRepository.GetRequestsByMentorId(mentorId, page, size);
        var response = result.Items.Where(q => q.MentorId == mentorId).Select(p => new RequestResponse
        {
            RequestId = p.Id,
            Title = p.Title,
            Start = p.Start,
            End = p.End,
            Status = RequestStatusEnum.Accepted,
            ProjectName = p.Project.Title
        }).ToList();
        var paginationParse = new Pagination<RequestResponse>
        {
            Items = response,
            PageIndex = page,
            PageSize = size,
            TotalItems = result.TotalItems,
            TotalPages = result.TotalPages
        };
        return paginationParse;
    }
    

    public async Task<bool> UpdateRequestStatus(Guid requestId, RequestStatusEnum status)
    {
        try
        {
            var request = await _requestRepository.GetRequestById(requestId);
            request.Status = status;
            return _requestRepository.Update(request);
        }
        catch (Exception e)
        {
            return false;
        }
    }
}