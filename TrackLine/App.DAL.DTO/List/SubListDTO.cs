using Base.Domain;

namespace App.DAL.DTO.List;

public class SubListDTO : DomainEntityId
{
    // FK HeadList
    public Guid HeadListId { get; set; } = default!;
    
    // Used as a default translation name (eng) or in user created objects
    public string DefaultTitle { get; set; } = default!;
    
    // Public list can be saw by other users
    public bool IsPublic { get; set; } = false;
}