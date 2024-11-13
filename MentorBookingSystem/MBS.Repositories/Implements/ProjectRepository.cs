using MBS.BusinessObject.Entities;
using MBS.BusinessObject.Pagination;
using MBS.DataAccess.DAO.Interfaces;
using MBS.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBS.DataAccess.Pagination;
using Microsoft.EntityFrameworkCore;

namespace MBS.Repositories.Implements
{
    public class ProjectRepository(IBaseDAO<Project> dao) : BaseRepository<Project>(dao), IProjectRepository
    {

        public async Task<Pagination<Project>> GetAllProjects(int page, int size)
        {
            return await _dao.GetPagingListAsync(
                page: page,
                size: size
                );
        }

        public async Task<Project?> GetProjectById(Guid projectId)
        {
            return await _dao.SingleOrDefaultAsync(
                predicate: x => x.Id == projectId,
                include: q => q.Include(x => x.Mentor).ThenInclude(m => m.User)
                );
        }

        public async Task<Pagination<Project>> GetProjectsByMentorId(string mentorId, int page, int size)
        {
            return await _dao.GetPagingListAsync(
                predicate: p => p.MentorId == mentorId,
                page: page,
                size: size
                );
        }
    }
}
