using MBS.DataAccess.Pagination;
using MBS.Services.Dtos;
using MBS.Services.Models.Responses.Requests;


namespace MBS.Services.Services.Interfaces;

public interface IRequestService
{
    Task<Pagination<RequestDto>> GetRequestsByProjectIdPaginationAsync(Guid projectId, int pageNumber, int pageSize, string sortOrder = "desc", string? projectStatus = null);
    public Task<Pagination<RequestResponse>> GetAllRequestByMentorId(string mentorId, int page, int size);

}