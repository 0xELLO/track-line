using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers.List;

public class ListItemMapper: BaseMapper<App.DAL.DTO.List.ListItemDTO, App.Domain.List.ListItem>
{
    public ListItemMapper(IMapper mapper) : base(mapper)
    {
    }
}