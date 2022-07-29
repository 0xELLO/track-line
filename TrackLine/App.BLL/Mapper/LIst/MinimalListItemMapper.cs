using AutoMapper;
using Base.DAL;

namespace App.BLL.Mapper.LIst;

public class MinimalListItemMapper:  BaseMapper<App.BLL.DTO.List.MinimalListItemDTO, App.DAL.DTO.List.ListItemDTO>
{
    public MinimalListItemMapper(IMapper mapper) : base(mapper)
    {
    }
}