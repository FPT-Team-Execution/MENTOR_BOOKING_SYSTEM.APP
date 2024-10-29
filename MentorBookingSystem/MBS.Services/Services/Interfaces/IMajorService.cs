using MBS.Services.Models;
using MBS.Services.Models.Requests.Group;
using MBS.Services.Models.Requests.Major;

namespace MBS.Services.Services.Interfaces;

public interface IMajorService
{
    public Task<IResponse> GetMajorsAsync(int page, int size);
    Task<IResponse> CreateNewMajorAsync(CreateNewMajorRequestModel request);

}