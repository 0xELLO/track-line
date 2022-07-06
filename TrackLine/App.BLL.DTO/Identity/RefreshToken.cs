﻿using Base.Domain;

namespace App.BLL.DTO.Identity;

public class RefreshToken : DomainEntityId
{
    public string Token { get; set; } = Guid.NewGuid().ToString() ;
    // UTC
    public DateTime ExpirationTime{ get; set; } = DateTime.UtcNow.AddMinutes(7);
    
    
    public string? PreviousToken { get; set; }
    // UTC
    public DateTime? PreviousExpirationTime{ get; set; }

    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}