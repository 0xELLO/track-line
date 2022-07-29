using Base.Domain;

namespace App.DAL.DTO.List;

public class ListItemInSubListDTO : DomainEntityId
{
    // FK SubList
    public Guid SubListId { get; set; } = default!;

    // FK ListObject
    public Guid ListObjectId { get; set; } = default!;

    // Position in sublist
    public int Position { get; set; } = default!;
}