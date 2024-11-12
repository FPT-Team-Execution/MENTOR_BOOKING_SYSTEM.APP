using MBS.Services.Models.Responses.Major;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using MBS.BusinessObject.Entities;
using MBS.Services.Dtos;

namespace MBS.Razor.Mappers
{
    public class MajorMapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<MentorMajor, MajorDto>()
                .Map(dest => dest.Name, src => src.Major.Name);
        }
    }
}