using MBS.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Services.Services.Interfaces
{
    public interface IRequestService
    {
        Task<IResponse> GetResponseAsync(int page, int size, string sortOrder);
    }
}
