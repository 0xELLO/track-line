using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain.List;

/// <summary>
/// Basic representation of every list object. Anime, films, books etc
/// </summary>
public class ListItem : DomainEntityMetaId
{
    // Used as a default translation name (eng) or in user created objects
    [MaxLength(128)]
    public string DefaultTitle { get; set; } = default!;

    // Unique indentifier
    [MaxLength(128)] 
    public string Code { get; set; } = default!;
    
    // Length of the object in episodes/pages etc
    [Range(1, 9999)]
    public int TotalLength { get; set; } = 1;

    // Opened to search engine/other users can find and add this object
    public bool IsPublic { get; set; } = false;
    
    // Is created by user or recieved externaly
    public bool IsCreatedByUser { get; set; } = false;

    public List<ListItemInSubList>? ListItemsInSubList { get; set; }
    public List<UserListItemProgress>? UserListItemProgresses { get; set; }

}