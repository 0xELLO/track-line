using App.BLL.DTO.List;

namespace App.Contracts.BLL.Services;

public interface IListItemService
{
    public Task AddNewListItem(MinimalListItemDTO listItem);
    public Task AddListItemToSubList(string listItemCode, string subListId);
    public Task MoveListItemToSubList(string listItemId, string targetSubListId);
    public Task ChangeListItemProgress(string listItemId);
}