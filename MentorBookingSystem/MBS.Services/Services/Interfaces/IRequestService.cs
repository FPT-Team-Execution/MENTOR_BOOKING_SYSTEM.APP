using MBS.BusinessObject.Entities;
using MBS.BusinessObject.Enums;
using MBS.DataAccess.Pagination;
using MBS.Services.Dtos;
using MBS.Services.Models.Responses.Requests;


namespace MBS.Services.Services.Interfaces;

public interface IRequestService
{
    Task<Pagination<RequestDto>> GetRequestsByProjectIdPaginationAsync(Guid projectId, int pageNumber, int pageSize, string sortOrder = "desc", string? projectStatus = null);
    public Task<Pagination<RequestDto>> GetAllRequestByMentorId(string mentorId, int page, int size);
    public Task<bool> UpdateRequestStatus(Guid requestId, RequestStatusEnum status);
    public Task<RequestDto> GetRequestById(Guid requestId);
    Task<bool> CreateProjectRequest(Request request);
    Task<IEnumerable<RequestDto>> GetRequestsByProjectId(Guid projectId, string? status = null);

    public Task<Pagination<RequestResponse>> GetAllRequestPagination(int page, int size, string sortOrder);

    public Task<Pagination<RequestResponse>> GetAllRequestByStudentId(string studentId, int page, int size);
}