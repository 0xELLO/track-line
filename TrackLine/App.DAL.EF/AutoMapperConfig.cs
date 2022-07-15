using App.DAL.DTO.Identity;
using App.DAL.DTO.List;
using App.Domain.List;
using AutoMapper;

namespace App.DAL.EF;

public class AutoMapperConfig: Profile
{
    public AutoMapperConfig()
    { 
        CreateMap<App.DAL.DTO.FooBar, App.Domain.FooBar>().ReverseMap();

        CreateMap<HeadListDTO, App.Domain.List.HeadList>().ReverseMap();       
        CreateMap<SubListDTO, App.Domain.List.SubList>().ReverseMap();       
        CreateMap<ListItemDTO, App.Domain.List.ListItem>().ReverseMap();       
        CreateMap<ListItemInSubListDTO, App.Domain.List.ListItemInSubList>().ReverseMap();       
        CreateMap<UserListItemProgressDTO, App.Domain.List.UserListItemProgress>().ReverseMap();       
        
        CreateMap<AppUserDTO, App.Domain.Identity.AppUser>().ReverseMap();
        CreateMap<RefreshTokenDTO, App.Domain.Identity.RefreshToken>().ReverseMap();
    }
}