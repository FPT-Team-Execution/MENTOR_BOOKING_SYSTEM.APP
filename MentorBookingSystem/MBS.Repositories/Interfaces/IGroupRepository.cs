using MBS.BusinessObject.Entities;
using MBS.BusinessObject.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBS.DataAccess.Pagination;

namespace MBS.Repositories.Interfaces
{
    public interface IGroupRepository : IBaseRepository<Group>
    {
        Task<Group> GetGroupByIdAsync(Guid id);
        Task<IEnumerable<Group>> GetGroupByProjectIdAsync(Guid projectId);
        Task<Pagination<Group>> GetGroupsByStudentId(string studentId, int page, int size, string sortOrder);
        Task<IEnumerable<Group>> GetGroupsByStudentId(string studentId, string? projectStatus = null);

        Task<Pagination<Group>> GetPagedListBaseAsync(int page, int size);
        Task<Group> GetGroupByProjectAndStudentIdAsync(Guid projectId, string studentId);
    }
}
