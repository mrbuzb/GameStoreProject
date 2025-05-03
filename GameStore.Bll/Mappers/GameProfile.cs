using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.Bll.Dto_s;
using GameStore.Dal.Entities;

namespace GameStore.Bll.Mappers;

public class GameProfile : Profile
{
    public GameProfile()
    {
        CreateMap<Game, GameDto>();

        CreateMap<GameDto, Game>()
            .ForMember(dest => dest.GameGenres, opt => opt.Ignore())
            .ForMember(dest => dest.GamePlatforms, opt => opt.Ignore());
    }
}
