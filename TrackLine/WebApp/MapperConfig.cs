using AutoMapper;

namespace WebApp;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<App.Public.DTO.v1.FooBar, App.BLL.DTO.FooBar>().ReverseMap();
        CreateMap<App.Public.DTO.v1.List.ExtendedListItem, App.BLL.DTO.List.ExtendedListItemDTO>().ReverseMap();
        CreateMap<App.Public.DTO.v1.List.MinimalListItem, App.BLL.DTO.List.MinimalListItemDTO>().ReverseMap();
        CreateMap<App.Public.DTO.v1.List.SubList, App.BLL.DTO.List.SubListDTO>().ReverseMap();
        CreateMap<App.Public.DTO.v1.List.HeadList, App.BLL.DTO.List.HeadListDTO>().ReverseMap();
    }
}