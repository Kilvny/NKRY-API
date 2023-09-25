using AutoMapper;
using NKRY_API.Domain.Entities;
using NKRY_API.Helpers;
using System.Data;
using NKRY_API.Models;

namespace NKRY_API.Profiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(
                dest => dest.MemberForHowLongInYears,
                opt => opt.MapFrom(src => src.CreatedAt.CalculateCurrentAge()));


            CreateMap<CreateUserDto, User>()
                .ForMember(
                dest => dest.UserName,
                opt => opt.MapFrom(src => src.Email));
        }
    }
}
