using System.ComponentModel;
using Base.Domain;

namespace App.Public.DTO.v1.List;

[DisplayName("SubList")]
public class SubList: DomainEntityId
{ 
    public string HeadListId { get; set; } = default!;
    
    // Used as a default translation name (eng) or in user created objects
    public string DefaultTitle { get; set; } = default!;
    
    // Public list can be saw by other users
    public bool IsPublic { get; set; } = false;
}