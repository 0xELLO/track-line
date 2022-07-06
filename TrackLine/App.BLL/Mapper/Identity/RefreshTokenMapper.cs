using AutoMapper;
using Base.DAL;

namespace App.BLL.Mapper.Identity;

public class RefreshTokenMapper: BaseMapper<App.BLL.DTO.Identity.RefreshToken, App.DAL.DTO.Identity.RefreshToken>
{
    public RefreshTokenMapper(IMapper mapper) : base(mapper)
    {
    }
}