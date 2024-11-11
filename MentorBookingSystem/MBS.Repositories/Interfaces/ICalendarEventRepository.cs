using MBS.BusinessObject.Entities;
using MBS.BusinessObject.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Repositories.Interfaces
{
    public interface ICalendarEventRepository : IBaseRepository<CalendarEvent>
    {
        Task<Pagination<CalendarEvent>> GetCalendarEventsByMentorIdPaginationAsync(
            string mentorId,
            DateTime? startDate = null,
            DateTime? endDate = null,
            string sortBy = "asc",
            int page = 1,
            int pageSize = 10);
        Task<IEnumerable<CalendarEvent>> GetCalendarEventsByMentorIdAsync(
            string mentorId,
            DateTime? startDate,
            DateTime? endDate
            );
        Task<CalendarEvent?> GetEventByIdAsync(string calendarEventId);
    }
}
