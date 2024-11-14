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
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        public StudentRepository(IBaseDAO<Student> dao) : base(dao)
        {
        }

        public async Task<Pagination<Student>> GetStudentsAsync(int page, int size, string sortOrder = "asc")
        {
            return await _dao.GetPagingListAsync(
                include: s => s.Include(x => x.User),
                orderBy: q => (sortOrder.ToLower() == "asc") ? q.OrderBy(x => x.User.FullName) : q.OrderByDescending(x => x.User.FullName),
                page: page,
                size: size
            );
        }

        public async Task<Student?> GetByUserIdAsync(string userId, Func<IQueryable<Student>, IIncludableQueryable<Student, object>> include = null)
        {
            return await _dao.SingleOrDefaultAsync(
                predicate: x => x.UserId == userId,
                include: include
            );
        }
        public async Task<Student?> GetStudentByIdAsync(string userId)
        {
            return await _dao.SingleOrDefaultAsync(
                predicate: x => x.UserId == userId,
                include: q => q.Include(x => x.User)
            );
        }


        public async Task<IEnumerable<Student>> GetStudents()
        {
            return await _dao.GetListAsync();

        }

    }
}
