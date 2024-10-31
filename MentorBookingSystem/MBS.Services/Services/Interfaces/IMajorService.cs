using MBS.Services.Models;
using MBS.Services.Models.Requests.Group;
using MBS.Services.Models.Requests.Major;

namespace MBS.Services.Services.Interfaces;

public interface IMajorService
{
    public Task<IResponse> GetMajorsAsync(int page, int size);
    public Task<IResponse> GetMentorMajorsAsync(GetMentorMajorsRequest request);
    Task<IResponse> CreateNewMajorAsync(CreateNewMajorRequestModel request);
}