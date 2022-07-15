using Base.Domain;

namespace App.DAL.DTO.List;

public class ListItemInSubListDTO : DomainEntityId
{
    // FK SubList
    public string SubListId { get; set; } = default!;

    // FK ListObject
    public string ListObjectId { get; set; } = default!;

    // Position in sublist
    public int Position { get; set; } = default!;
}