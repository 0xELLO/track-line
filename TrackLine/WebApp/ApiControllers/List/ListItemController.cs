using App.Contracts.BLL;
using App.Public.DTO.v1.List;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ApiControllers.List;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
[ApiController]
public class ListItemController : ControllerBase
{
    private readonly IAppBLL _bll;
    private readonly ILogger<ListItemController> _logger;
    private readonly Mapper _mapper;

    public ListItemController(IAppBLL bll, ILogger<ListItemController> logger, Mapper mapper)
    {
        _bll = bll;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> CreateListItem(MinimalListItem minimalListItem)
    {
        var result = await _bll.ListItemService.AddNewListItem(_mapper.MinimalListItemMapper.Map(minimalListItem));
        await _bll.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("subListId")]
    public async Task<ActionResult<IEnumerable<ExtendedListItem>>> GetExtendedListItems(Guid subListId)
    {
        var result = await _bll.ListItemService.GetListItemsBySubList(subListId);
        return Ok(result);
    }

    [HttpPost]
    public async Task ChangeProgress(string listItemId, string progress)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public async Task<ActionResult<ExtendedListItem>> AddListItemToSubList(string listItemCode, Guid subListId)
    {
        var res = await _bll.ListItemService.AddListItemToSubList(listItemCode, subListId);
        await _bll.SaveChangesAsync();
        return Ok(res);
    }



}