using MBS.Services.Models;
using MBS.Services.Models.Requests.Group;
using MBS.Services.Models.Requests.Position;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Services.Services.Interfaces
{
    public interface IPositionService
    {
        public Task<IResponse> GetPositionsAsync(int page, int size);
        Task<IResponse> CreateNewPositionAsync(CreateNewPositionRequestModel request);


    }
}
