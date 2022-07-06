using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain.Identity;

public class RefreshToken : DomainEntityId
{
    [StringLength(36), MinLength(36)]
    public string Token { get; set; } = Guid.NewGuid().ToString() ;
    // UTC
    public DateTime ExpirationTime{ get; set; } = DateTime.UtcNow.AddMinutes(7);
    
    
    [StringLength(36), MinLength(36)]
    public string? PreviousToken { get; set; }
    // UTC
    public DateTime? PreviousExpirationTime{ get; set; }

    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}