using AutoMapper;

namespace App.BLL;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<App.BLL.DTO.FooBar, App.DAL.DTO.FooBar>().ReverseMap();
        CreateMap<App.BLL.DTO.List.MinimalListItemDTO, App.DAL.DTO.List.ListItemDTO>().ReverseMap();
        CreateMap<App.BLL.DTO.Identity.RefreshTokenDTO, App.DAL.DTO.Identity.RefreshTokenDTO>().ReverseMap();
        CreateMap<App.BLL.DTO.Identity.AppUserDTO, App.DAL.DTO.Identity.AppUserDTO>().ReverseMap();
        CreateMap<App.BLL.DTO.List.HeadListDTO, App.DAL.DTO.List.HeadListDTO>().ReverseMap();
        CreateMap<App.BLL.DTO.List.SubListDTO, App.DAL.DTO.List.SubListDTO>().ReverseMap();
    }
}