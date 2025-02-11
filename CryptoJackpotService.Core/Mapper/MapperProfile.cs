using AutoMapper;
using CryptoJackpotService.Data.Database.Models;
using CryptoJackpotService.Models.DTO;
using CryptoJackpotService.Models.Request;

namespace CryptoJackpotService.Core.Mapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        MapDto();
    }

    private void MapDto()
    {
        CreateMap<User, UserDto>();
        CreateMap<CreateUserRequest, User>();

        CreateMap<Country, CountryDto>();
        CreateMap<Role, RoleDto>();
    }
}