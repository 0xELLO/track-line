using Base.Domain.Identity;

namespace App.Domain.Identity;

public class AppUser : BaseUser
{
    public ICollection<RefreshToken>? RefreshTokens { get; set; }
}