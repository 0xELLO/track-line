using App.Contracts.BLL;
using App.Domain.Identity;
using App.Public.DTO.v1.List;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ApiControllers.List;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/list/[controller]/[action]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class SubListController : ControllerBase
{
    private readonly IAppBLL _bll;
    private readonly ILogger<SubListController> _logger;
    private readonly Mapper _mapper;
    private readonly UserManager<AppUser> _userManager;

    public SubListController(IAppBLL bll, ILogger<SubListController> logger, Mapper mapper, UserManager<AppUser> userManager)
    {
        _bll = bll;
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
    }
    
    //api/v1/list/GetHeadLists
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SubList>>> GetSubList(string headListId)
    {
        return Ok((await _bll.SubListService.getSubListsByHeadListId(headListId)).Select(x => _mapper.SubListMapper.Map(x)));
    }

    [HttpPost]
    public async Task<ActionResult<SubList>> PostSubList(SubList subList)
    {
        var result = _bll.SubListService.Add(_mapper.SubListMapper.Map(subList));
        await _bll.SaveChangesAsync();
        return Ok(_mapper.SubListMapper.Map(result));
    }
}