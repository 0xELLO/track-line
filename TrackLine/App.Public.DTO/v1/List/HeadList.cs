using System.ComponentModel;
using Base.Domain;

namespace App.Public.DTO.v1.List;
[DisplayName("HeadList")]
public class HeadList : DomainEntityId
{
    // FK AppUser
    public Guid? AppUserId { get; set; } = default!;
    
    // Used as a default translation name (eng) or in user created objects
    public string DefaultTitle { get; set; } = default!;
}