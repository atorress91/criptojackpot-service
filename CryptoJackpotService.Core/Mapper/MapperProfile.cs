using AutoMapper;
using CryptoJackpotService.Data.Database.Custom;
using CryptoJackpotService.Data.Database.Models;
using CryptoJackpotService.Models.Configuration;
using CryptoJackpotService.Models.DTO.Country;
using CryptoJackpotService.Models.DTO.Prize;
using CryptoJackpotService.Models.DTO.Role;
using CryptoJackpotService.Models.DTO.User;
using CryptoJackpotService.Models.DTO.UserReferral;
using CryptoJackpotService.Models.Request.Pagination;
using CryptoJackpotService.Models.Request.Prize;
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
        CreateMap<PaginationRequest, Pagination>();
        
        CreateMap<User, UserDto>();
        CreateMap<CreateUserRequest, User>();
        CreateMap<UserReferralRequest, UserReferral>()
            .ForMember(dest => dest.UsedSecurityCode, 
                opt => opt.MapFrom(src => src.ReferralCode));

        CreateMap<Country, CountryDto>();
        CreateMap<Role, RoleDto>();
        CreateMap<UserReferral, UserReferralDto>();
        CreateMap<UserReferralWithStats, UserReferralDto>();
        CreateMap<IEnumerable<UserReferralWithStats>, UserReferralStatsDto>()
            .ForMember(dest => dest.TotalEarnings, 
                opt => opt.MapFrom(src => src.Count() * 10))
            .ForMember(dest => dest.LastMonthEarnings, 
                opt => opt.MapFrom(src => src.Count(r => r.RegisterDate >= DateTime.Now.AddMonths(-1)) * 10))
            .ForMember(dest => dest.Referrals, 
                opt => opt.MapFrom(src => src));

        // Prize mappings
        CreateMap<CreatePrizeRequest, Prize>();
        CreateMap<PrizeImageRequest, PrizeImage>();
        CreateMap<Prize, PrizeDto>();
        CreateMap<PrizeImage, PrizeImageDto>();
    }
}