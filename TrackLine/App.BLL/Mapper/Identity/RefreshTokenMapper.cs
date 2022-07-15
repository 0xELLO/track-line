using AutoMapper;
using Base.DAL;

namespace App.BLL.Mapper.Identity;

public class RefreshTokenMapper: BaseMapper<App.BLL.DTO.Identity.RefreshTokenDTO, App.DAL.DTO.Identity.RefreshTokenDTO>
{
    public RefreshTokenMapper(IMapper mapper) : base(mapper)
    {
    }
}