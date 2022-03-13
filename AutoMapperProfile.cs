using AutoMapper;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Models;

namespace dotnet_rpg
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, GetCharacterDto>();
            CreateMap<AddCharacterDto, Character>();
            CreateMap<Character, GetCharacterNameOnlyDto>();
            CreateMap<GetPagedCharacterDto, Character>();
            CreateMap<Character, GetPagedCharacterDto>();

        }
    }
}