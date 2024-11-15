using MBS.BusinessObject.Entities;
using MBS.DataAccess.DAO.Interfaces;
using MBS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBS.BusinessObject.Pagination;
using MBS.DataAccess.Pagination;

namespace MBS.Repositories.Implements
{
    public class MentorRepository(IBaseDAO<Mentor> dao) : BaseRepository<Mentor>(dao), IMentorRepository
    {
        public async Task<Mentor?> GetMentorByIdAsync(string mentorId)
        {
            return await _dao.SingleOrDefaultAsync(
              predicate:  m => m.UserId == mentorId,
              include: source => source.Include(m=>m.User)
            );
        }

        public Task<Mentor?> GetByUserIdAsync(string userId, Func<IQueryable<Mentor>, IIncludableQueryable<Mentor, object>> include = null)
        {
            return _dao.SingleOrDefaultAsync(
                predicate: x => x.UserId == userId,
                include: include
                );
        }

        public async Task<Pagination<Mentor>> GetMentorsPaginationAsync(int page, int size)
        {
            return await _dao.GetPagingListAsync(
                include: source => source.Include(m => m.User),
                page: page,
                size: size
            );
        }

        public async Task<IEnumerable<Mentor>> GetMentorsAsync()
        {
            return await _dao.GetListAsync(
                include: s => s.Include(m => m.User)
                );
        }


    }
}
