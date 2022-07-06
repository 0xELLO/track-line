using Base.DAL;
using AutoMapper;
using App.DAL.DTO;

namespace App.DAL.EF.Mappers;

public class FooBarMapper: BaseMapper<App.DAL.DTO.FooBar,App.Domain.FooBar>
{
    public FooBarMapper(IMapper mapper) : base(mapper)
    {
    }
}