using App.BLL.DTO.List;

namespace App.Contracts.BLL.Services;

public interface IListItemService
{
    public Task<MinimalListItemDTO> AddNewListItem(MinimalListItemDTO listItem, bool noTracking = true);
    public Task<ExtendedListItemDTO> AddListItemToSubList(string listItemCode, Guid subListId, Guid appUserId, bool noTracking = true);
    public Task MoveListItemToSubList(Guid listItemId, Guid targetSubListId, bool noTracking = true);
    public Task ChangeListItemProgress(Guid listItemId, bool noTracking = true);

    public Task<IEnumerable<ExtendedListItemDTO>> GetListItemsBySubList(Guid subListId,  bool noTracking = true);

}