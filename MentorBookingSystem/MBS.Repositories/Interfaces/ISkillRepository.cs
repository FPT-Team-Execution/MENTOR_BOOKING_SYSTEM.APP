using MBS.BusinessObject.Entities;
using MBS.BusinessObject.Pagination;
using MBS.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ISkillRepository : IBaseRepository<Skill>
{
    Task<Pagination<Skill>> GetPagedListAsyncByMentorId(int page, int size, string mentorId);

    Task<Skill> GetSkillByIdAsync(Guid id);

}
