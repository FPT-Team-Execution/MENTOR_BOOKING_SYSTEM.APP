using MBS.BusinessObject.Entities;
using MBS.BusinessObject.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.DataAccess.Repositories.Interfaces
{
    public interface IProjectRepository : IBaseRepository<Project>
    {
        Task<Pagination<Project>> GetProjectsByMentorId(string mentorId, int page, int size, string sortOrder);
        Task<Pagination<Project>> GetAllProjects(int page, int size);

    }
}
