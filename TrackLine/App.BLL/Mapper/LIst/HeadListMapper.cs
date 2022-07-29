using AutoMapper;
using Base.DAL;

namespace App.BLL.Mapper.LIst;

public class HeadListMapper:  BaseMapper<App.BLL.DTO.List.HeadListDTO, App.DAL.DTO.List.HeadListDTO>
{
    public HeadListMapper(IMapper mapper) : base(mapper)
    {
    }
}