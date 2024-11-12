using Mapster;
using MBS.DataAccess.Pagination;
using MBS.Repositories.Interfaces;
using MBS.Services.Dtos;
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
}