using MBS.BusinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.DataAccess.Repositories.Interfaces
{
    public interface IMeetingMemberRepository : IBaseRepository<MeetingMember>
    {
        public Task<IEnumerable<MeetingMember>> GetMeetingMemberByMeetingIdAsync(Guid id);
        public Task<MeetingMember?> GetMeetingMemberByIdAsync(Guid id);

    }
}
