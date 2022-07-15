using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Domain;

namespace App.Domain.List;

public class HeadList : DomainEntityMetaId
{
    // FK AppUser
    public AppUser AppUser { get; set; } = default!;
    public string AppUserId { get; set; } = default!;
    
    // Used as a default translation name (eng) or in user created objects
    [MaxLength(128)]
    public string DefaultTitle { get; set; } = default!;
    
    public List<SubList>? SubLists { get; set; }
}