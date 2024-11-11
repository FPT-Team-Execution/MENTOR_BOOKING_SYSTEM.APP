using MBS.BusinessObject.Entities;
using MBS.BusinessObject.Pagination;
using MBS.DataAccess.DAO.Interfaces;
using MBS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Repositories.Implements
{
    public class CalendarEventRepository(IBaseDAO<CalendarEvent> dao) : BaseRepository<CalendarEvent>(dao), ICalendarEventRepository
    {
        public async Task<Pagination<CalendarEvent>> GetCalendarEventsByMentorIdPaginationAsync(string mentorId, DateTime? startDate = null, DateTime? endDate = null,
            string sortBy = "asc", int page = 1, int pageSize = 10)
        {
            Expression<Func<CalendarEvent, bool>> predicate = ce => ce.MentorId == mentorId &&
                                                                    (startDate == null || ce.Start >= startDate) &&
                                                                    (endDate == null || ce.End <= endDate);
            Func<IQueryable<CalendarEvent>, IOrderedQueryable<CalendarEvent>> orderBy = (sortBy.ToLower() == "asc") ?
                ce => ce.OrderBy(x => x.Start) : ce => ce.OrderByDescending(x => x.Start);
            return await _dao.GetPagingListAsync(
                predicate: predicate,
                orderBy: orderBy,
                page: page,
                size: pageSize
                );
        }

        public async Task<IEnumerable<CalendarEvent>> GetCalendarEventsByMentorIdAsync(string mentorId, DateTime? startDate, DateTime? endDate)
        {
            Expression<Func<CalendarEvent, bool>> predicate = ce => ce.MentorId == mentorId &&
                                                                    (startDate == null || ce.Start >= startDate) &&
                                                                    (endDate == null || ce.End <= endDate);
            Func<IQueryable<CalendarEvent>, IOrderedQueryable<CalendarEvent>> orderBy = ce => ce.OrderBy(x => x.Start);
            return await _dao.GetListAsync(
                predicate: predicate,
                orderBy: orderBy
            );
        }

        public async Task<CalendarEvent?> GetEventByIdAsync(string calendarEventId)
        {
            return await _dao.SingleOrDefaultAsync(
                predicate: x => x.Id == calendarEventId,
                include: x => x.Include(m => m.Meeting).Include(m => m.Mentor)
                );
        }
    }
}
