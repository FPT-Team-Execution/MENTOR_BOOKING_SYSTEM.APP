using MBS.BusinessObject.Entities;
using MBS.BusinessObject.Pagination;
using MBS.DataAccess.DAO.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.DataAccess.Repositories.Implements
{
    public class SkillRepository(IBaseDAO<Skill> dao) : BaseRepository<Skill>(dao), ISkillRepository
    {
        public Task<Skill?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Pagination<Skill>> GetPagedListAsyncByMentorId(int page, int size, string mentorId)
        {
            return await _dao.GetPagingListAsync(
                predicate: s => s.MentorId == mentorId,
                page: page,
                size: size
            );
        }

        public async Task<Skill> GetSkillByIdAsync(Guid id)
        {
            return await _dao.SingleOrDefaultAsync(
                m => m.Id == id,
                include: x => x.Include(y => y.Mentor)

                );
        }
    }
}
