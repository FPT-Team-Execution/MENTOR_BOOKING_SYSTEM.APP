using MBS.Services.Dtos;

namespace MBS.Services.Services.Interfaces;

public interface IProgressService
{
    Task<IEnumerable<ProgressDto>> GetProgressByProjectIdAsync(Guid projectId);

    Task<(double Percent, IEnumerable<ProgressDto> Complete, IEnumerable<ProgressDto> NotComplete)>
        GetCompleteProgressPercent(Guid projectId);
}