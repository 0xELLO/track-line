using Base.Domain;

namespace App.Domain.List;

public class ListItemInSubList : DomainEntityMetaId
{
    // FK SubList
    public SubList SubList { get; set; } = default!;
    public string SubListId { get; set; } = default!;

    // FK ListObject
    public ListItem ListItem { get; set; } = default!;
    public string ListObjectId { get; set; } = default!;

    // Position in sublist
    public int Position { get; set; } = default!;
}