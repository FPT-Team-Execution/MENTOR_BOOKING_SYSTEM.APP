using MBS.BusinessObject.Entities;
using MBS.BusinessObject.Pagination;
using MBS.DataAccess.DAO.Interfaces;
using MBS.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Repositories.Implements
{
    public class PositionRepository : BaseRepository<Position>, IPositionRepository
    {
        public PositionRepository(IBaseDAO<Position> dao) : base(dao)
        {
        }

        public async Task<Position> GetPositionByIdAsync(Guid id)
        {
            return await _dao.SingleOrDefaultAsync(x => x.Id == id);

        }

        public async Task<Pagination<Position>> GetPositions(int page, int size)
        {
            return await _dao.GetPagingListAsync(
                page: page,
                size: size
                );
        }
    }
}
