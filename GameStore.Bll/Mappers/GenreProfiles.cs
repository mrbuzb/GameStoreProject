using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.Bll.Dto_s;
using GameStore.Dal.Entities;

namespace GameStore.Bll.Mappers;

public class GenreProfiles : Profile
{
    public GenreProfiles()
    {
        CreateMap<GenreCreateDto, Genre>().ReverseMap();
        CreateMap<GenreDto, Genre>().ReverseMap();
        CreateMap<GenreGetDto, Genre>().ReverseMap();
    }
}
