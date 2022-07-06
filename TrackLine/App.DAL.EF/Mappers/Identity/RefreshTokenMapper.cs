using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers.Identity;

public class RefreshTokenMapper: BaseMapper<App.DAL.DTO.Identity.RefreshToken, App.Domain.Identity.RefreshToken>
{
    public RefreshTokenMapper(IMapper mapper) : base(mapper)
    {
    }
}