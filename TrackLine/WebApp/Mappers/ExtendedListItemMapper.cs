using AutoMapper;
using Base.DAL;

namespace WebApp.Mappers;

public class ExtendedListItemMapper: BaseMapper<App.Public.DTO.v1.List.ExtendedListItem, App.BLL.DTO.List.ExtendedListItemDTO>
{
    public ExtendedListItemMapper(IMapper mapper) : base(mapper)
    {
    }
}