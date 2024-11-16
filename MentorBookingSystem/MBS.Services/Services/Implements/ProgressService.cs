using Mapster;
using MBS.BusinessObject.Entities;
using MBS.Repositories.Interfaces;
using MBS.Services.Dtos;
using MBS.Services.Services.Interfaces;

namespace MBS.Services.Services.Implements;

public class ProgressService : IProgressService
{
    private readonly IProgressRepository _progressRepository;
    private readonly IProjectRepository _projectRepository;
    
    public ProgressService(IProgressRepository progressRepository, IProjectRepository projectRepository)
    {
        _progressRepository = progressRepository;
        _projectRepository = projectRepository;
    }

    public async Task<ProgressDto?> GetProgressIdAsync(Guid id)
    {
        var progress = await _progressRepository.GetProgressByIdAsync(id);
        return progress.Adapt<ProgressDto>();
    }

    public async Task<bool> UpdateProgress(ProgressDto progressDto)
    {
        var progress = await _progressRepository.GetProgressByIdAsync(progressDto.Id);
        progress.IsComplete = progressDto.IsComplete;
        return _progressRepository.Update(progress);
    }

    public async Task<bool> DeleteProgress(Guid id)
    {        
        var progress = await _progressRepository.GetProgressByIdAsync(id);
        return _progressRepository.Delete(progress);
    }

    public async Task<bool> CreateProgress(Progress newProgress)
    {
        
        return await _progressRepository.CreateAsync(newProgress);
    }

    public async Task<IEnumerable<ProgressDto>> GetProgressByProjectIdAsync(Guid projectId)
    {
        var progresses = await _progressRepository.GetProgressesByProjectId(projectId);
        return progresses.Adapt<List<ProgressDto>>();
    }
    public async Task<(double Percent, IEnumerable<ProgressDto> Complete, IEnumerable<ProgressDto> NotComplete)> GetCompleteProgressPercent(Guid projectId)
    {
        // Retrieve and sort progresses
        var progresses = await _progressRepository.GetProgressesByProjectId(projectId);
        var progressList = progresses.ToList();

        if (!progressList.Any())
        {
            return (0, Enumerable.Empty<ProgressDto>(), Enumerable.Empty<ProgressDto>());
        }

        // Separate completed and uncompleted progress items
        var completed = progressList.Where(p => p.IsComplete).ToList();
        var uncompleted = progressList.Where(p => !p.IsComplete).ToList();

        // Calculate completion percentage
        double percentComplete = (double)completed.Count / progressList.Count * 100;

        return (percentComplete, completed.Adapt<List<ProgressDto>>(), uncompleted.Adapt<List<ProgressDto>>());
    }

}