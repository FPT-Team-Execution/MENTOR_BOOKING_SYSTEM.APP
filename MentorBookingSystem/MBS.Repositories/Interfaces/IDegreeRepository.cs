using MBS.BusinessObject.Entities;
using MBS.BusinessObject.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBS.DataAccess.Pagination;

namespace MBS.Repositories.Interfaces
{
    public interface IDegreeRepository : IBaseRepository<Degree>
    {
        Task<Pagination<Degree>> GetDegreesByMentorId(string mentorId, int page, int size);
    }
}
