using App.Domain.List;
using Base.Domain.Identity;

namespace App.Domain.Identity;

public class AppUser : BaseUser
{
    public ICollection<HeadList>? HeadLists { get; set; }
    public ICollection<UserListItemProgress>? UserListObjectProgresses { get; set; }
    public ICollection<RefreshToken>? RefreshTokens { get; set; }
}