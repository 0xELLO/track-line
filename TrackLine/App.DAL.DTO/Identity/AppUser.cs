using Base.Domain;

namespace App.DAL.DTO.Identity;

public class AppUser : DomainEntityId
{
    public ICollection<RefreshToken>? RefreshTokens { get; set; }
}