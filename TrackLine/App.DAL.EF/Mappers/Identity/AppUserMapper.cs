using AutoMapper;
using Base.Contracts;
using Base.DAL;

namespace App.DAL.EF.Mappers.Identity;

public class AppUserMapper : BaseMapper<App.DAL.DTO.Identity.AppUserDTO, App.Domain.Identity.AppUser>
{
    public AppUserMapper(IMapper mapper) : base(mapper)
    {
    }
}