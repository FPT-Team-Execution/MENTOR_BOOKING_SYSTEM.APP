using MBS.Services.Models;

namespace MBS.Services.Services.Interfaces;

public interface IMajorService
{
    public Task<IResponse> GetMajorsAsync(int page, int size);
}