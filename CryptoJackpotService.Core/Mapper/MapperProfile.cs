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
        CreateMap<UserReferralRequest, UserReferral>()
            .ForMember(dest => dest.UsedSecurityCode, 
                opt => opt.MapFrom(src => src.ReferralCode));

        CreateMap<Country, CountryDto>();
        CreateMap<Role, RoleDto>();
        CreateMap<UserReferral, UserReferralDto>();
    }
}