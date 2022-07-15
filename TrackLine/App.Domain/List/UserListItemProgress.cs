using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Domain;

namespace App.Domain.List;

public class UserListItemProgress : DomainEntityMetaId
{
    // FK ListObject
    public ListItem ListItem { get; set; } = default!;
    public string ListObjectId { get; set; } = default!;
    
    // FK AppUser
    public AppUser AppUser { get; set; } = default!;
    public string AppUserId { get; set; } = default!;

    [Range(0, 9999)]
    public int Progress { get; set; } = 0;
    
    [Range(0, 9999)]
    public int TimesFinished { get; set; } = 0;
}