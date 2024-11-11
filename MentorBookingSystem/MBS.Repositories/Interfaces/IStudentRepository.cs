using MBS.BusinessObject.Entities;
using MBS.BusinessObject.Pagination;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Repositories.Interfaces
{
    public interface IStudentRepository : IBaseRepository<Student>
    {

        public Task<Pagination<Student>> GetStudentsAsync(int page, int size, string sortOrder);
        Task<Student?> GetByUserIdAsync(string userId, Func<IQueryable<Student>, IIncludableQueryable<Student, object>> include = null);

        Task<IEnumerable<Student>> GetStudents();

    }
}
