using App.BLL.DTO.List;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.Repositories.List;
using Base.BLL;

namespace App.BLL.Services;

public class ListItemService : IListItemService
{
    private readonly IListItemRepository _listItemRepository;
    private readonly IListItemInSubListRepository _listItemInSubListRepository;
    private readonly IUserListItemProgressRepository _userListItemProgressRepository;

    public ListItemService(IListItemRepository listItemRepository, IListItemInSubListRepository listItemInSubListRepository,
        IUserListItemProgressRepository userListItemProgressRepository)
    {
        _listItemRepository = listItemRepository;
        _listItemInSubListRepository = listItemInSubListRepository;
        _userListItemProgressRepository = userListItemProgressRepository;
    }


    public Task AddNewListItem(MinimalListItemDTO listItem)
    {
        throw new NotImplementedException();
    }

    public Task AddListItemToSubList(string listItemCode, string subListId)
    {
        throw new NotImplementedException();
    }

    public Task MoveListItemToSubList(string listItemId, string targetSubListId)
    {
        throw new NotImplementedException();
    }

    public Task ChangeListItemProgress(string listItemId)
    {
        throw new NotImplementedException();
    }
}