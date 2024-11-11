using MBS.BusinessObject.Entities;
using MBS.DataAccess.DAO.Interfaces;
using MBS.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Repositories.Implements
{
    public class MeetingRepository(IBaseDAO<Meeting> dao) : BaseRepository<Meeting>(dao), IMeetingRepository
    {
        public async Task<IEnumerable<Meeting>> GetMeetingsByRequest(Guid requestId)
        {
            return await _dao.GetListAsync(
               predicate: m => m.RequestId == requestId);
        }

        public async Task<IEnumerable<Meeting>> GetMeetingsByRequests(IEnumerable<Guid> requestIds)
        {
            return await _dao.GetListAsync(
                predicate: m => requestIds.Contains(m.Id));
        }
    }
}
