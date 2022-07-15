using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers.List;

public class SubListMapper: BaseMapper<App.DAL.DTO.List.SubListDTO, App.Domain.List.SubList>
{
    public SubListMapper(IMapper mapper) : base(mapper)
    {
    }
}