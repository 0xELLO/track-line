using Base.Domain;

namespace App.DAL.DTO.Identity;

public class AppUserDTO : DomainEntityId
{
    public ICollection<RefreshTokenDTO>? RefreshTokens { get; set; }
}