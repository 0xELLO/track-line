using Base.DAL;
using AutoMapper;

namespace App.BLL.Mapper;

public class FooBarMapper:  BaseMapper<App.BLL.DTO.FooBar, App.DAL.DTO.FooBar>
{
    public FooBarMapper(IMapper mapper) : base(mapper)
    {
    }
}