using AutoMapper;
using MBS.Services.Models.Responses.Major;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Razor.Mappers
{
    public class MajorMapper : Profile
    {
        public MajorMapper()
        {
            CreateMap<Major, MajorResponseDto>();
        }
    }
}
