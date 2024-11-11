using MBS.Services.Models.Responses.Student;
using MBS.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBS.Services.Models.Responses.Project;
using MBS.Services.Models.Requests.Position;
using MBS.Services.Models.Requests.Project;

namespace MBS.Services.Services.Interfaces
{
    public interface IProjectService
    {
        Task<IResponse> GetProjectAsync(int page, int size, string sortOrder);

        Task<IResponse> CreateProjectAsync(CreateProjectModel createProjectModel);
    }
}
