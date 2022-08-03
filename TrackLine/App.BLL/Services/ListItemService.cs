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
    public async Task<MinimalListItemDTO> AddNewListItem(MinimalListItemDTO listItem, bool noTracking = true)
    {
        return _mapper.Map(_listItemRepository.Add(_mapper.Map(listItem)!))!;;
    }

    public async Task<ExtendedListItemDTO> AddListItemToSubList(string listItemCode, Guid subListId, bool noTracking = true)
    {
        var listItem = new ListItemDTO();
        if ((!await _listItemRepository.Exists(listItemCode, noTracking)))
        {
            // TODO search service dependency
        }
        else
        {
            listItem = ((await _listItemRepository.GetByCode(listItemCode, noTracking))!);
        }
        
        _listItemInSubListRepository.Add(new ListItemInSubListDTO
        {
            SubListId = subListId,
            ListObjectId = listItem.Id,
            Position = 1
        });
        await FixPosition(subListId);

        return new ExtendedListItemDTO
        {
            Id = listItem.Id,
            DefaultTitle = listItem.DefaultTitle,
            Code = listItem.Code,
            TotalLength = listItem.TotalLength,
            IsPublic = listItem.IsPublic,
            IsCreatedByUser = listItem.IsCreatedByUser,
            Position = 1,
            Progress = 0,
            TimesFinished = 0
        };
    }

    private async Task FixPosition(Guid subListId)
    {
        var listItems = await _listItemInSubListRepository.GetAllBySubListId(subListId);
        listItems = listItems.OrderBy(x => x.Position);
        var prevPosition = 0;
        foreach (var item in listItems)
        {
            if (item.Position == prevPosition)
            {
                item.Position += 1;
            }

            prevPosition = item.Position;
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
    

     public async Task<IEnumerable<ExtendedListItemDTO>> GetListItemsBySubList(Guid subListId, bool noTracking)
     {
         var items = await _listItemInSubListRepository.GetAllBySubListId(subListId);
         var eListItems = new List<ExtendedListItemDTO>();
         
         foreach (var item in items)
         {
             var listItem = await _listItemRepository.GetById(item.Id);
             var progress = await _userListItemProgressRepository.GetByListObjectId(listItem!.Id);
             // TODO automapper merge two objects together
             // Mapper https://entityframeworkcore.com/knowledge-base/43614417/merge-multiple-sources-into-a-single-destination
             eListItems.Add(new ExtendedListItemDTO
             {
                 DefaultTitle = listItem.DefaultTitle,
                 Code = listItem.Code,
                 TotalLength = listItem.TotalLength,
                 IsPublic = listItem.IsPublic,
                 IsCreatedByUser = listItem.IsCreatedByUser,
                 Position = item.Position,
                 Progress = progress.Progress,
                 TimesFinished = progress.TimesFinished
             });
         }
         
         return eListItems;
     }
}