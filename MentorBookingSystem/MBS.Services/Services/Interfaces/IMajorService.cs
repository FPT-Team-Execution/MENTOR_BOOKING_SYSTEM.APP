using MBS.Services.Dtos;
using MBS.Services.Models;
using MBS.Services.Models.Requests.Major;
using MBS.Services.Models.Responses.Major;

namespace MBS.Services.Services.Interfaces;

public interface IMajorService
{
    public Task<IEnumerable<MajorDto>> GetAllMajors();
    public Task<IResponse> GetMajorsAsync(int page, int size);
    public Task<IResponse> GetMentorMajorsAsync(GetMentorMajorsRequest request);
    Task<IResponse> CreateNewMajorAsync(CreateNewMajorRequestModel request);
}