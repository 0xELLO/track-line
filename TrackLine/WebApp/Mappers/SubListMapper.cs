using AutoMapper;
using Base.DAL;

namespace WebApp.Mappers;

public class SubListMapper: BaseMapper<App.Public.DTO.v1.List.SubList, App.BLL.DTO.List.SubListDTO>
{
    public SubListMapper(IMapper mapper) : base(mapper)
    {
    }
}