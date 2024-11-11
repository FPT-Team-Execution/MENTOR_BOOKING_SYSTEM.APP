using MBS.BusinessObject.Entities;
using MBS.BusinessObject.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.DataAccess.Repositories.Interfaces
{
    public interface IPositionRepository : IBaseRepository<Position>
    {
        Task<Position> GetPositionByIdAsync(Guid id);
        Task<Pagination<Position>> GetPositions(int page, int size);
    }
}
