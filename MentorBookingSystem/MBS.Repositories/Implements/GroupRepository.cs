using MBS.BusinessObject.Entities;
using MBS.BusinessObject.Pagination;
using MBS.DataAccess.DAO.Interfaces;
using MBS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBS.BusinessObject.Enums;

namespace MBS.Repositories.Implements
{
    public class GroupRepository(IBaseDAO<Group> dao) : BaseRepository<Group>(dao), IGroupRepository
    {
        public Task<Group> GetGroupByIdAsync(Guid id)
        {
            return _dao.SingleOrDefaultAsync(x => x.Id == id);
        }

        public Task<Group> GetGroupByProjectAndStudentIdAsync(Guid projectId, string studentId)
        {
            return _dao.SingleOrDefaultAsync(x => x.StudentId == studentId && x.ProjectId == projectId);
        }

        public async Task<IEnumerable<Group>> GetGroupByProjectIdAsync(Guid project)
        {
            return await _dao.GetListAsync(
                predicate: a => a.ProjectId == project,
                include: q => q.Include(x => x.Position).Include(s => s.Student).ThenInclude(st => st.User)
            );
        }

        public async Task<Pagination<Group>> GetGroupsByStudentId(string studentId, int page, int size,
            string sortOrder)
        {
            return await _dao.GetPagingListAsync(
                predicate: g => g.StudentId == studentId,
                include: p => p.Include(x => x.Project),
                orderBy: p =>
                    (sortOrder.ToLower() == "asc")
                        ? p.OrderBy(x => x.Project.CreatedOn)
                        : p.OrderByDescending(x => x.Project.CreatedOn),
                page: page,
                size: size
            );
        }

        public async Task<IEnumerable<Group>> GetGroupsByStudentId(string studentId, string? projectStatus = null)
        {
            var groups = await _dao.GetListAsync(
                predicate: g => g.StudentId == studentId,
                include: p => p.Include(x => x.Project)
            );
            if (!string.IsNullOrEmpty(projectStatus))
            {
                var projectStatusEnum = Enum.Parse<ProjectStatusEnum>(projectStatus!, true);
                groups.Where(g => g.Project.Status == projectStatusEnum).ToList();
            }

            return groups;
        }

        public async Task<Pagination<Group>> GetPagedListBaseAsync(int page, int size)
        {
            return await _dao.GetPagingListAsync
            (
                include: p => p.Include(
                    x => x.Project).Include(y => y.Student).Include(z => z.Position)
            );
        }
    }
}