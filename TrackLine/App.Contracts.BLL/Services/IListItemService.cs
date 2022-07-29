using App.BLL.DTO.List;

namespace App.Contracts.BLL.Services;

public interface IListItemService
{
    public Task<MinimalListItemDTO> AddNewListItem(MinimalListItemDTO listItem, bool noTracking = true);
    public Task<ExtendedListItemDTO> AddListItemToSubList(string listItemCode, string subListId, bool noTracking = true);
    public Task MoveListItemToSubList(string listItemId, string targetSubListId, bool noTracking = true);
    public Task ChangeListItemProgress(string listItemId, bool noTracking = true);

    public Task<IEnumerable<ExtendedListItemDTO>> GetListItemsBySubList(string subListId,  bool noTracking = true);
}