using AutoMapper;
using Base.DAL;

namespace App.BLL.Mapper.Identity;

public class AppUserMapper : BaseMapper<App.BLL.DTO.Identity.AppUserDTO, App.DAL.DTO.Identity.AppUserDTO>
{
    public AppUserMapper(IMapper mapper) : base(mapper)
    {
    }
}