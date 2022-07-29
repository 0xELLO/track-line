using AutoMapper;
using Base.DAL;

namespace WebApp.Mappers;

public class MinimalListItemMapper: BaseMapper<App.Public.DTO.v1.List.MinimalListItem, App.BLL.DTO.List.MinimalListItemDTO>
{
    public MinimalListItemMapper(IMapper mapper) : base(mapper)
    {
    }
}