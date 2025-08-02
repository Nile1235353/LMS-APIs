using AutoMapper;
using Microsoft.AspNetCore.Identity;
using RGL_LMS.Dto;
using RGL_LMS.Models;

namespace RGL_LMS
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<UserForRegistrationDto, IdentityUser>().ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));
            CreateMap<Users, UserDto>().ReverseMap();

            CreateMap<UserDto, Users>()
    .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role));

            CreateMap<Courses, CourseDto>().ReverseMap();


        }
    }
}
