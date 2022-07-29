using AutoMapper;
using Base.DAL;

namespace WebApp.Mappers;

public class HeadListMapper: BaseMapper<App.Public.DTO.v1.List.HeadList, App.BLL.DTO.List.HeadListDTO>
{
    public HeadListMapper(IMapper mapper) : base(mapper)
    {
    }
}