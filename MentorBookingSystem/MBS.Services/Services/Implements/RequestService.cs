using Azure;
using Mapster;
using MBS.BusinessObject.Entities;
using MBS.BusinessObject.Enums;
using MBS.DataAccess.Pagination;
using MBS.Repositories.Interfaces;
using MBS.Services.Dtos;
using MBS.Services.Models.Responses.Requests;
using MBS.Services.Services.Interfaces;

namespace MBS.Services.Services.Implements;

public class RequestService : IRequestService
{
    private readonly IRequestRepository _requestRepository;
    private readonly ICalendarEventRepository _eventRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IStudentRepository _studentRepository;


    public RequestService(
        IRequestRepository requestRepository,
        ICalendarEventRepository eventRepository,
        IGroupRepository groupRepository,
        IStudentRepository studentRepository)
    {
        _requestRepository = requestRepository;
        _eventRepository = eventRepository;
        _groupRepository = groupRepository;
        _studentRepository = studentRepository;
    }

    public async Task<Pagination<RequestDto>> GetRequestsByProjectIdPaginationAsync(Guid projectId, int pageNumber,
        int pageSize, string sortOrder = "desc", string? projectStatus = null)
    {
        var request =
            await _requestRepository.GetRequestByProjectIdPaginationAsync(projectId, pageNumber, pageSize, sortOrder,
                projectStatus);
        return request.Adapt<Pagination<RequestDto>>();
    }

    public async Task<Pagination<RequestDto>> GetAllRequestByMentorId(string mentorId, int page, int size)
    {
        var result = await _requestRepository.GetRequestsByMentorId(mentorId, page, size);
        var response = result.Items.Where(q => q.MentorId == mentorId).Select(p => new RequestDto
        {
            Id = p.Id,
            Title = p.Title,
            Start = p.Start,
            End = p.End,
            Status = p.Status.ToString(),
            ProjectName = p.Project.Title
        }).ToList();
        var paginationParse = new Pagination<RequestDto>
        {
            Items = response,
            PageIndex = page,
            PageSize = size,
            TotalItems = result.TotalItems,
            TotalPages = result.TotalPages
        };
        return paginationParse;
    }

    public async Task<bool> CreateProjectRequest(Request request)
    {
        try
        {
            //Create request
            var addResult = await _requestRepository.CreateAsync(request);
            return addResult;
        }
        catch (Exception e)
        {
            return false;
        }
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

    public async Task<RequestDto> GetRequestById(Guid requestId)
    {
        try
        {
            var request = await _requestRepository.GetRequestById(requestId);
            if (request == null)
            {
                throw new NullReferenceException("Request does not exist");
            }

            return request.Adapt<RequestDto>();
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public async Task<IEnumerable<RequestDto>> GetRequestsByProjectId(Guid projectId, string? status = null)
    {
        var request = await _requestRepository.GetRequestByProjectIdAsync(projectId, status);
        return request.Adapt<List<RequestDto>>();
    }

    public async Task<Pagination<RequestResponse>> GetAllRequestPagination(int page, int size, string sortOder)
    {
        var result = await _requestRepository.GetRequestPaginationAsync(page, size, sortOder);
        //var response = result.Items.ToList();
        var response = result.Items.Select(p => new RequestResponse
        {
            RequestId = p.Id,
            Title = p.Title,
            Start = p.Start,
            End = p.End,
            Status = p.Status,
            ProjectName = p.Project.Title ?? "No Project"   
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

        //return request.Adapt<Pagination<RequestResponse>>();
    }
}