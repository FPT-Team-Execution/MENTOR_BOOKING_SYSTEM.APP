using MBS.Services.Models.Requests.Position;
using MBS.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBS.Services.Models.Requests.Skill;

namespace MBS.Services.Services.Interfaces
{
    public interface ISkillService
    {
        public Task<IResponse> GetSkillsAsync(int page, int size);
        Task<IResponse> CreateNewSkillAsync(CreateNewSkillRequestDTO request);
    }
}
