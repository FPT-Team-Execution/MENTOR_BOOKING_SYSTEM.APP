using MBS.BusinessObject.Entities;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.DataAccess.Repositories.Implements
{
    public class MeetingMemberRepository(IBaseDAO<MeetingMember> dao) : BaseRepository<MeetingMember>(dao), IMeetingMemberRepository
    {
        public async Task<IEnumerable<MeetingMember>> GetMeetingMemberByMeetingIdAsync(Guid id)
        {
            return await _dao.GetListAsync(
                predicate: mm => mm.MeetingId == id,
                include: q => q.Include(mm => mm.Student)
                );
        }

        public async Task<MeetingMember?> GetMeetingMemberByIdAsync(Guid id)
        {
            return await _dao.SingleOrDefaultAsync(
                predicate: mm => mm.MeetingId == id,
                include: q => q.Include(mm => mm.Meeting)
            );
        }
    }
}
