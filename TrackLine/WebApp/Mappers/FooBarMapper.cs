using AutoMapper;
using Base.DAL;

namespace WebApp.Mappers;

public class FooBarMapper: BaseMapper<App.Public.DTO.v1.FooBar, App.BLL.DTO.FooBar>
{
    public FooBarMapper(IMapper mapper) : base(mapper)
    {
    }
}