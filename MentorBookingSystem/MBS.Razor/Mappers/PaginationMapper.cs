using AutoMapper;
using MBS.BusinessObject.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Razor.Mappers
{
    public class PaginationMapper : Profile
    {
        public PaginationMapper()
        {
            CreateMap(typeof(Pagination<>), typeof(Pagination<>));
        }
    }
}
