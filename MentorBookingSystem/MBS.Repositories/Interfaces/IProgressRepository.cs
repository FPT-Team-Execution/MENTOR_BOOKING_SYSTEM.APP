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
    public interface IProgressRepository : IBaseRepository<Progress>
    {
        Task<Pagination<Progress>> GetProgressesAsync(Guid projectId, int pageNumber, int pageSize, string sortOrder);
        Task<IEnumerable<Progress>> GetProgressesByProjectId(Guid projectId);
        Task<Progress?> GetProgressByIdAsync(Guid id);
        Task<bool> CreateProgressesAsync(Guid projectId, IEnumerable<string> progressTitleList);


    }
}
