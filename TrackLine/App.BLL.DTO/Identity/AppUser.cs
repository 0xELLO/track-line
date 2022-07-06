using Base.Domain;

namespace App.BLL.DTO.Identity;

public class AppUser : DomainEntityId
{
    public ICollection<RefreshToken>? RefreshTokens { get; set; }
}