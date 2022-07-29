using System.IdentityModel.Tokens.Jwt;
using App.BLL.DTO.List;
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
public class HeadListController : ControllerBase
{
    private readonly IAppBLL _bll;
    private readonly ILogger<HeadListController> _logger;
    private readonly Mapper _mapper;
    private readonly UserManager<AppUser> _userManager;

    public HeadListController(IAppBLL bll, ILogger<HeadListController> logger, Mapper mapper, UserManager<AppUser> userManager)
    {
        _bll = bll;
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
    }
    
    // TODO Error handle
    // TODO description 
    
    //api/v1/list/GetHeadLists
    [HttpGet]
    public async Task<ActionResult<IEnumerable<HeadList>>> GetHeadLists([FromHeader] string authorization)
    {
       var appUser =  await _bll.AppUserService.GetUserFromJwt(authorization, _userManager);
       if (appUser == null)
       {
           return BadRequest();
       }
       
       return Ok((await _bll.HeadListService.getHeadListsByUserId(appUser!.Id.ToString()))
           .Select(x => _mapper.HeadListMapper.Map(x)));
    }

    [HttpPost]
    public async Task<ActionResult<HeadList>> PostHeadList(HeadList headList, [FromHeader] string authorization)
    {
        var appUser =  await _bll.AppUserService.GetUserFromJwt(authorization, _userManager);
        if (appUser == null)
        {
            return BadRequest();
        }

        headList.AppUserId = appUser.Id.ToString();
        var result = _bll.HeadListService.Add(_mapper.HeadListMapper.Map(headList));
        await _bll.SaveChangesAsync();
        return Ok(_mapper.HeadListMapper.Map(result));
    }
    
    
    

}