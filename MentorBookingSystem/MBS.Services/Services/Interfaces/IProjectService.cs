using MBS.Services.Models.Responses.Student;
using MBS.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBS.Services.Dtos;
using MBS.Services.Models.Responses.Project;
using MBS.Services.Models.Requests.Position;
using MBS.Services.Models.Requests.Project;
using MBS.DataAccess.Pagination;

namespace MBS.Services.Services.Interfaces
{
    public interface IProjectService
    {
        Task<IResponse> CreateProjectAsync(CreateProjectModel createProjectModel);
        Task<ProjectDto?> GetProjectByIdAsync(Guid projectId);
        public Task<Pagination<ProjectResponse>> GetProjectAsync(int page, int size, string search);

        public Task<Pagination<ProjectResponse>> GetAllProjectByMentorId(string mentorId, int page, int size);


    }
}
