using AutoMapper;
using API.Entities;
using API.DTOs;
using API.Extensions;
using System.Linq;
namespace API.Helpers
{
    public class AutoMapersProfile:Profile
    {
        public AutoMapersProfile()
        {
            CreateMap<AppUser,MemberDTO>()
            .ForMember(dest=>dest.PhotoUrl,opt=>opt.MapFrom(src=>
            src.Photos.FirstOrDefault(x=>x.IsMain).Url))
            .ForMember(dest=>dest.Age,opt=>opt.MapFrom(src=>src.DateOfBirth.CalculateAge()));
            CreateMap<Photo,PhotoDTO>();
        }
    }
}