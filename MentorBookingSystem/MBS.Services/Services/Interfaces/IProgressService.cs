using MBS.BusinessObject.Entities;
using MBS.Services.Dtos;

namespace MBS.Services.Services.Interfaces;

public interface IProgressService
{
    Task<ProgressDto?> GetProgressIdAsync(Guid id);
    Task<bool> UpdateProgress(ProgressDto progressDto);
    Task<bool> DeleteProgress(Guid id);
    Task<bool> CreateProgress(Progress progress);

    Task<IEnumerable<ProgressDto>> GetProgressByProjectIdAsync(Guid projectId);

    Task<(double Percent, IEnumerable<ProgressDto> Complete, IEnumerable<ProgressDto> NotComplete)>
        GetCompleteProgressPercent(Guid projectId);
}