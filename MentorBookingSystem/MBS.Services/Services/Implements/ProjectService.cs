using MBS.Services.Constants;
using MBS.Services.Models;
using MBS.Services.Models.Responses.Mentor;
using MBS.Services.Models.Responses.Project;
using MBS.Services.Services.Interfaces;
using MBS.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using MBS.Repositories.Interfaces;
using MBS.Services.Dtos;

namespace MBS.Services.Services.Implements
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }
        public async Task<ProjectDto?> GetProjectByIdAsync(Guid projectId)
        {
            var project = await _projectRepository.GetProjectById(projectId);
            return project.Adapt<ProjectDto>();
        }

        public async Task<IResponse> GetProjectAsync(int page, int size, string search)
        {
            var url = ApiEndPoints.ProjectUrl.Replace("{page}",page.ToString()).Replace("{pageSize}",size.ToString()).Replace("{search}",search);
            var result = await WebUtils.GetAsync(url);
            var response = WebUtils.HandleResponse<BaseModel<Pagination<ProjectResponse>>>(result);
            return response;
        }
        
    }
}
