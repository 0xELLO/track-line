using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers.List;

public class HeadListMapper: BaseMapper<App.DAL.DTO.List.HeadListDTO, App.Domain.List.HeadList>
{
    public HeadListMapper(IMapper mapper) : base(mapper)
    {
    }
}