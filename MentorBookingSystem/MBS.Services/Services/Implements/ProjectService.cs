using MBS.Services.Constants;
using MBS.Services.Models;
using MBS.Services.Models.Requests.Project;
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
using MBS.DataAccess.Pagination;
using MBS.BusinessObject.Entities;
using MBS.Services.Models.Responses.Group;

namespace MBS.Services.Services.Implements
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }
        
        public async Task<IResponse> CreateProjectAsync(CreateProjectModel createProjectModel)
        {
            var newProject = new Project();
            newProject.Id = Guid.NewGuid();
            newProject.Title = createProjectModel.Title;
            newProject.Description = createProjectModel.Description;
            newProject.Semester = createProjectModel.Semester;
            newProject.Status = BusinessObject.Enums.ProjectStatusEnum.Activated;
            newProject.MentorId = createProjectModel.MentorId;
            newProject.DueDate = DateTime.Now.AddDays(30);
            _projectRepository.CreateAsync(newProject);

            var result = await WebUtils.PostAsync(
           ApiEndPoints.ProjectUrl,
           newProject,
           token: WebUtils.AccessToken
                );
            var response = WebUtils.HandleResponse<BaseModel<ProjectResponse>>(result);
            return response;
        }
        
        public async Task<ProjectDto?> GetProjectByIdAsync(Guid projectId)
        {
            var project = await _projectRepository.GetProjectById(projectId);
            return project.Adapt<ProjectDto>();
        }

        public async Task<Pagination<ProjectResponse>> GetProjectAsync(int page, int size, string search)
        {
            var result = await _projectRepository.GetAllProjects(page, size);
            return result.Adapt<Pagination<ProjectResponse>>();
        }


        public async Task<Pagination<ProjectResponse>> GetAllProjectByMentorId(string mentorId, int page, int size)
        {
            var result = await _projectRepository.GetAllAsync();
            var projectResponseByMentor = new List<Project>();
            foreach (var project in result)
            {
                if(project.MentorId == mentorId)
                {
                    var projectAdd = new Project();
                    projectAdd.Title = project.Title;
                    projectAdd.Description = project.Description;
                    projectAdd.Status = project.Status;
                    projectAdd.Semester = project.Semester;
                    projectAdd.DueDate = project.DueDate;
                    projectResponseByMentor.Add(projectAdd);
                }

            }
            var paginatedItems = projectResponseByMentor
        .Skip((page - 1) * size)
        .Take(size)
        .ToList();

            // T?o ??i t??ng phân trang ?? tr? v?
            var projectPagination = new Pagination<ProjectResponse>
            {
                Items = (IEnumerable<ProjectResponse>)paginatedItems,
                PageIndex = page,
                PageSize = size,
                TotalItems = projectResponseByMentor.Count,
                TotalPages = (int)Math.Ceiling((double)projectResponseByMentor.Count / size)
            };

            return projectPagination;
        }
    }
}
