using MBS.BusinessObject.Entities;
using MBS.BusinessObject.Pagination;
using MBS.DataAccess.DAO.Interfaces;
using MBS.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Repositories.Implements
{
    public class DegreeRepository(IBaseDAO<Degree> dao) : BaseRepository<Degree>(dao), IDegreeRepository
    {
        public Task<Pagination<Degree>> GetDegreesByMentorId(string mentorId, int page, int size)
        {
            return dao.GetPagingListAsync(x => x.MentorId == mentorId, page: page, size: size);
        }
    }
}
