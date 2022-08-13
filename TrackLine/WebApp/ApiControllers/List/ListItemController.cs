using App.Contracts.BLL;
using App.Domain.Identity;
using App.Public.DTO.v1.List;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ApiControllers.List;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/list/[controller]/[action]")]
[ApiController]
public class ListItemController : ControllerBase
{
    private readonly IAppBLL _bll;
    private readonly ILogger<ListItemController> _logger;
    private readonly Mapper _mapper;
    private readonly UserManager<AppUser> _userManager;

    public ListItemController(IAppBLL bll, ILogger<ListItemController> logger, Mapper mapper, UserManager<AppUser> userManager)
    {
        _bll = bll;
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> CreateListItem(MinimalListItem minimalListItem)
    {
        var result = await _bll.ListItemService.AddNewListItem(_mapper.MinimalListItemMapper.Map(minimalListItem));
        await _bll.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<ExtendedListItem>>> GetExtendedListItems(Guid id)
    {
        var result = await _bll.ListItemService.GetListItemsBySubList(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task ChangeProgress(string listItemId, string progress)
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    public async Task<ActionResult<ExtendedListItem>> AddListItemToSubList([FromHeader] string authorization, string listItemCode, Guid subListId)
    {
        var appUser = await _bll.AppUserService.GetUserFromJwt(authorization, _userManager);
        var res = await _bll.ListItemService.AddListItemToSubList(listItemCode, subListId, appUser!.Id);
        await _bll.SaveChangesAsync();
        return Ok(res);
    }



}