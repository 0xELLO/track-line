using Base.Domain;

namespace App.BLL.DTO.List;

public class HeadListDTO : DomainEntityId
{
    // FK AppUser
    public string AppUserId { get; set; } = default!;
    
    // Used as a default translation name (eng) or in user created objects
    public string DefaultTitle { get; set; } = default!;
}