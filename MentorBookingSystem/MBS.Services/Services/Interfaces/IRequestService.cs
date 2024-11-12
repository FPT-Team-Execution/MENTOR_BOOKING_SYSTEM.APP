using MBS.DataAccess.Pagination;
using MBS.Services.Dtos;


namespace MBS.Services.Services.Interfaces;

public interface IRequestService
{
    Task<Pagination<RequestDto>> GetRequestsByProjectIdPaginationAsync(Guid projectId, int pageNumber, int pageSize, string sortOrder = "desc", string? projectStatus = null);
}