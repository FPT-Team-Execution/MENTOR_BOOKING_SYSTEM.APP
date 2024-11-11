using MBS.BusinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Repositories.Interfaces
{
    public interface IMeetingRepository : IBaseRepository<Meeting>
    {
        Task<IEnumerable<Meeting>> GetMeetingsByRequests(IEnumerable<Guid> requestIds);
        Task<IEnumerable<Meeting>> GetMeetingsByRequest(Guid requestId);


    }
}
