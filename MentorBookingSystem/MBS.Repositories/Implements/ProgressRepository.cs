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
using MBS.DataAccess.Pagination;

namespace MBS.Repositories.Implements
{
    public class ProgressRepository(IBaseDAO<Progress> dao) : BaseRepository<Progress>(dao), IProgressRepository
    {
        public async Task<Pagination<Progress>> GetProgressesAsync(Guid projectId, int pageNumber, int pageSize, string sortOrder = "asc")
        {
            var progress = await _dao.GetPagingListAsync(
                predicate: p => p.ProjectId == projectId,
                orderBy: q => sortOrder == "asc" ? q.OrderBy(p => p.CreatedOn) : q.OrderByDescending(p => p.CreatedOn),
                include: q => q.Include(p => p.Project),
                page: pageNumber,
                size: pageSize
                );
            return progress;
        }

        public async Task<IEnumerable<Progress>> GetProgressesByProjectId(Guid projectId)
        {
            var progress = await _dao.GetListAsync(
                predicate: p => p.ProjectId == projectId,
                orderBy: q => q.OrderByDescending(p => p.CreatedOn)
            );
            return progress;
        }

        public async Task<Progress?> GetProgressByIdAsync(Guid id)
        {
            return await _dao.SingleOrDefaultAsync(predicate: p => p.Id == id);
        }

        public async Task<bool> CreateProgressesAsync(Guid projectId, IEnumerable<string> progressTitleList)
        {
            var progresses = progressTitleList.Select(p => new Progress
            {
                Id = Guid.NewGuid(),
                Name = p,
                ProjectId = projectId,
                IsComplete = false
            }).ToList();
            var addResult = await _dao.InsertRangeAsync(progresses);
            return addResult > 0;
        }
    }
}
