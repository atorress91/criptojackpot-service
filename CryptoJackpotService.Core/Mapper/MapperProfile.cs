using AutoMapper;
using CryptoJackpotService.Data.Database.Custom;
using CryptoJackpotService.Data.Database.Models;
using CryptoJackpotService.Models.DTO.Country;
using CryptoJackpotService.Models.DTO.Role;
using CryptoJackpotService.Models.DTO.User;
using CryptoJackpotService.Models.DTO.UserReferral;
using CryptoJackpotService.Models.Request.Referral;
using CryptoJackpotService.Models.Request.User;

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
        CreateMap<UserReferralWithStats, UserReferralStatsDto>();
    }
}