using AutoMapper;

namespace App.BLL;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<App.BLL.DTO.FooBar, App.DAL.DTO.FooBar>().ReverseMap();
        CreateMap<App.BLL.DTO.Identity.AppUserDTO, App.DAL.DTO.Identity.AppUserDTO>().ReverseMap();
        CreateMap<App.BLL.DTO.Identity.RefreshTokenDTO, App.DAL.DTO.Identity.RefreshTokenDTO>().ReverseMap();
    }
}