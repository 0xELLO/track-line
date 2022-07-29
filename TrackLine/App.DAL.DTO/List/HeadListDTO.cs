using Base.Domain;

namespace App.DAL.DTO.List;

public class HeadListDTO : DomainEntityId
{
    // FK AppUser
    public Guid AppUserId { get; set; } = default!;
    
    // Used as a default translation name (eng) or in user created objects
    public string DefaultTitle { get; set; } = default!;
}
