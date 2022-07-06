using AutoMapper;

namespace WebApp;

public class WebAutoMapperConfig : Profile
{
    public WebAutoMapperConfig()
    {
        CreateMap<App.Public.DTO.v1.FooBar, App.BLL.DTO.FooBar>().ReverseMap();
    }
}