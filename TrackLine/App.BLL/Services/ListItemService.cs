using App.BLL.DTO.List;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.Repositories.List;
using App.DAL.DTO.List;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class ListItemService : IListItemService
{
    private readonly IListItemRepository _listItemRepository;
    private readonly IListItemInSubListRepository _listItemInSubListRepository;
    private readonly IUserListItemProgressRepository _userListItemProgressRepository;
    private readonly IMapper<MinimalListItemDTO, ListItemDTO> _mapper;

    public ListItemService(IListItemRepository listItemRepository, IListItemInSubListRepository listItemInSubListRepository,
        IUserListItemProgressRepository userListItemProgressRepository, IMapper<MinimalListItemDTO, ListItemDTO> mapper)
    {
        _listItemRepository = listItemRepository;
        _listItemInSubListRepository = listItemInSubListRepository;
        _userListItemProgressRepository = userListItemProgressRepository;
        _mapper = mapper;
    }

    // TODO handle null
    // Adds new listItem item object
    public async Task<MinimalListItemDTO> AddNewListItem(MinimalListItemDTO listItem, bool noTracking = true)
    {
        return _mapper.Map(_listItemRepository.Add(_mapper.Map(listItem)!))!;;
    }
    
    /// <summary>
    /// Establishes connection between listItem and subList
    /// </summary>
    /// <param name="listItemCode">Unique list item identifier, may also be external (got from search engine and not yet be added into DB)</param>
    /// <param name="subListId"></param>
    /// <param name="appUserId"></param>
    /// <param name="noTracking"></param>
    /// <returns>Extended list item object</returns>
    public async Task<ExtendedListItemDTO> AddListItemToSubList(string listItemCode, Guid subListId, Guid appUserId, bool noTracking = true)
    {
        var listItem = new ListItemDTO();
        // if item doesn't exist in DB find it via external search engine first, add to DBa and then connect to subList
        if (!(await _listItemRepository.Exists(listItemCode, noTracking)))
        {
            // TODO search service dependency
        }
        // if exist find
        else
        {
            listItem = ((await _listItemRepository.GetByCode(listItemCode, noTracking))!);
        }
        
        // get progress to merge it into Extended list item DTO later
        _userListItemProgressRepository.Add(new UserListItemProgressDTO
        {
            ListItemId = listItem.Id,
            AppUserId = appUserId,
            Progress = 123,
            TimesFinished = 123
        });
        
        // get position to merge it into Extended list item DTO later
        _listItemInSubListRepository.Add(new ListItemInSubListDTO
        {
            SubListId = subListId,
            ListItemId = listItem.Id,
            Position = 1
        });
        
        // FIX position of old elements so that new item postion would be 1  
        await UpdatePosition(subListId, listItem.Id, 1);

        return new ExtendedListItemDTO
        {
            Id = listItem.Id,
            DefaultTitle = listItem.DefaultTitle,
            Code = listItem.Code,
            TotalLength = listItem.TotalLength,
            IsPublic = listItem.IsPublic,
            IsCreatedByUser = listItem.IsCreatedByUser,
            Position = 1,
            Progress = 1,
            TimesFinished = 0
        };
    }

    
    /// <summary>
    /// Updates position of listItem in sublist when new Item is added
    /// </summary>
    /// <param name="subListId">subList id to update</param>
    /// <param name="listItemId">Newly added list item</param>
    /// <param name="newPosition">Position of newly added list item</param>
    public async Task UpdatePosition(Guid subListId, Guid listItemId, int newPosition)
    {
        var listItems = await _listItemInSubListRepository.GetAllBySubListId(subListId);
        foreach (var item in listItems)
        {
            if (item.Position >= newPosition && item.ListItemId != listItemId)
            {
                item.Position += 1;
                _listItemInSubListRepository.Update(item);
            }

            if (listItemId == item.ListItemId)
            {
                item.Position = newPosition;
                _listItemInSubListRepository.Update(item);
            }
        }
    }

    public Task MoveListItemToSubList(Guid listItemId, Guid targetSubListId, bool noTracking = true)
    {
        throw new NotImplementedException();
    }

    public Task ChangeListItemProgress(Guid listItemId, bool noTracking = true)
    {
        throw new NotImplementedException();
        
    }

    /// <summary>
    /// Gets all list item in subList id 
    /// </summary>
    /// <param name="subListId"></param>
    /// <param name="noTracking"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ExtendedListItemDTO>> GetListItemsBySubList(Guid subListId, bool noTracking)
     {
         var listItemsInSubLists = await _listItemInSubListRepository.GetAllBySubListId(subListId);
         var eListItems = new List<ExtendedListItemDTO>();
         foreach (var listItemInSubList in listItemsInSubLists)
         {
             var listItem = await _listItemRepository.GetById(listItemInSubList.ListItemId);
             var progress = await _userListItemProgressRepository.GetByListItemId(listItem!.Id);

             // TODO automapper merge two objects together
             // Mapper https://entityframeworkcore.com/knowledge-base/43614417/merge-multiple-sources-into-a-single-destination
             eListItems.Add(new ExtendedListItemDTO
             {
                 Id = listItem!.Id,
                 DefaultTitle = listItem.DefaultTitle,
                 Code = listItem.Code,
                 TotalLength = listItem.TotalLength,
                 IsPublic = listItem.IsPublic,
                 IsCreatedByUser = listItem.IsCreatedByUser,
                 Position = listItemInSubList.Position,
                 Progress = progress.Progress,
                 TimesFinished = progress.TimesFinished
             });
         }
         
         return eListItems;
     }
}