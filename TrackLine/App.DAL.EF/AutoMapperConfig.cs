using App.DAL.DTO.Identity;
using AutoMapper;

namespace App.DAL.EF;

public class AutoMapperConfig: Profile
{
    public AutoMapperConfig()
    {
        CreateMap<App.DAL.DTO.FooBar, App.Domain.FooBar>().ReverseMap();
        CreateMap<AppUser, App.Domain.Identity.AppUser>().ReverseMap();
        CreateMap<RefreshToken, App.Domain.Identity.RefreshToken>().ReverseMap();
    }
}