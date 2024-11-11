using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Razor.Mappers
{
    public class ProjectMapper : Profile
    {
        public ProjectMapper()
        {
            //CreateMap<Project, ProjectResponseDTO>()
            //    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}
