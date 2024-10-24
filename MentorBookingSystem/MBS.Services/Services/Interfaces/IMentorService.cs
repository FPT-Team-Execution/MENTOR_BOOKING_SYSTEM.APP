﻿using MBS.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Services.Services.Interfaces
{
    public interface IMentorService
    {
        public Task<IResponse> GetMentorsAsync(int page, int size);
    }
}