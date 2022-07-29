using AutoMapper;
using Base.DAL;

namespace App.BLL.Mapper.LIst;

public class SubListMapper:  BaseMapper<App.BLL.DTO.List.SubListDTO, App.DAL.DTO.List.SubListDTO>
{
    public SubListMapper(IMapper mapper) : base(mapper)
    {
    }
}