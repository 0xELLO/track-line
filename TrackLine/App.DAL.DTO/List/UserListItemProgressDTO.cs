using Base.Domain;

namespace App.DAL.DTO.List;

public class UserListItemProgressDTO : DomainEntityId
{
    // FK ListObject
    public string ListObjectId { get; set; } = default!;
    
    // FK AppUser
    public string AppUserId { get; set; } = default!;

    public int Progress { get; set; } = 0;
    
    public int TimesFinished { get; set; } = 0;
}