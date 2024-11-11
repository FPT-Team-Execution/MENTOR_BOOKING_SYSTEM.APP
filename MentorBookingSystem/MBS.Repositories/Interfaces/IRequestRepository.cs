using MBS.BusinessObject.Entities;
using MBS.BusinessObject.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Repositories.Interfaces
{
    public interface IRequestRepository : IBaseRepository<Request>
    {
        Task<IEnumerable<Request>> GetRequestByProjectIdAsync(Guid projectId, string? status = null);
        Task<Pagination<Request>> GetRequestByProjectIdPaginationAsync(Guid projectId, int page, int size, string sortOrder, string? requestStatus);
        Task<Pagination<Request>> GetRequestByUserIdPaginationAsync(string userId, int page, int size, string sortOrder, string? requestStatus);
        Task<Pagination<Request>> GetRequestPaginationAsync(int page, int size, string sortOrder);
        Task<Request?> GetRequestById(Guid id);

    }
}
