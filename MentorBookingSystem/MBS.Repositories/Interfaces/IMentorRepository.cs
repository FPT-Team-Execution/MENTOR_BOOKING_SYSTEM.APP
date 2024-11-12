using MBS.BusinessObject.Entities;
using MBS.BusinessObject.Pagination;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBS.DataAccess.Pagination;

namespace MBS.Repositories.Interfaces
{
    public interface IMentorRepository : IBaseRepository<Mentor>
    {
        Task<Pagination<Mentor>> GetMentorsAsync(int page, int size);

        Task<Mentor?> GetByUserIdAsync(string userId,
            Func<IQueryable<Mentor>, IIncludableQueryable<Mentor, object>> include = null);

        Task<Mentor?> GetMentorByIdAsync(string mentorId);
    }
}