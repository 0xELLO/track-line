using App.Contracts.BLL;
using App.Public.DTO.v1.List;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ApiControllers;

/// <summary>
/// FooBar RESTful service
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
[ApiController]
public class SearchController : ControllerBase
{
    private readonly IAppBLL _bll;
    private readonly ILogger<SearchController> _logger;
    private readonly Mapper _mapper;
    
    public SearchController(IAppBLL bll, ILogger<SearchController> logger, Mapper mapper)
    {
        _bll = bll;
        _logger = logger;
        _mapper = mapper;
    }
    
    
    [HttpGet("{expression}")]
    public async Task<ActionResult<IEnumerable<MinimalListItem>>> TestSearch(string expression)
    {
        var res = await _bll.SearchService.SearchLocal(expression);
        return Ok(res.Select(x => _mapper.MinimalListItemMapper.Map(x)));
    }
}