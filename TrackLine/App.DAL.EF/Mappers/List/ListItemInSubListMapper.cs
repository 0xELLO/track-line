using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers.List;

public class ListItemInSubListMapper: BaseMapper<App.DAL.DTO.List.ListItemInSubListDTO, App.Domain.List.ListItemInSubList>
{
    public ListItemInSubListMapper(IMapper mapper) : base(mapper)
    {
    }
}