using Base.Domain;

namespace App.BLL.DTO.Identity;

public class AppUserDTO : DomainEntityId
{
    public ICollection<RefreshTokenDTO>? RefreshTokens { get; set; }
}