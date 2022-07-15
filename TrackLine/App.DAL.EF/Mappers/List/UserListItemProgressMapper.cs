using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers.List;

public class UserListItemProgressMapper: BaseMapper<App.DAL.DTO.List.UserListItemProgressDTO, App.Domain.List.UserListItemProgress>
{
    public UserListItemProgressMapper(IMapper mapper) : base(mapper)
    {
    }
}