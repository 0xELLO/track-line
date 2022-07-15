using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain.List;

public class SubList : DomainEntityMetaId
{
    // FK HeadList
    public HeadList HeadList { get; set; } = default!;
    public string HeadListId { get; set; } = default!;
    
    // Used as a default translation name (eng) or in user created objects
    [MaxLength(128)]
    public string DefaultTitle { get; set; } = default!;
    
    // Public list can be saw by other users
    public bool IsPublic { get; set; } = false;
    
    public List<ListItemInSubList>? ListObjectsInSubList { get; set; }
}