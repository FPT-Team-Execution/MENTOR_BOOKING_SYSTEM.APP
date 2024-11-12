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
    public interface IMajorRepository : IBaseRepository<Major>
    {
        Task<Major> GetMajorByIdAsync(Guid majorId);
        Task<Pagination<Major>> GetPagedListBaseAsync(int page, int size);
    }
}
