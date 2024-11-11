using MBS.BusinessObject.Entities;
using MBS.BusinessObject.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.DataAccess.Repositories.Interfaces
{
    public interface IPointTransactionRepository : IBaseRepository<PointTransaction>
    {
        Task<Pagination<PointTransaction>> GetTransactionByStudentIdPageList(string studentId, int page, int size);
        Task<Pagination<PointTransaction>> GetAllPageListAsync(int page, int size);
    }
}
