using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.Bll.Dto_s;
using GameStore.Dal.Entities;

namespace GameStore.Bll.Mappers;

public class PlatformProfile : Profile
{
    public PlatformProfile()
    {
        CreateMap<PlatformDto, Platform>().ReverseMap();
    }
}
