#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;
using App.Public.DTO.v1;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using FooBar = App.Public.DTO.v1.FooBar;

namespace WebApp.ApiControllers;

/// TODO https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags
/// TODO proper user data ownership control => access granting

/// <summary>
/// FooBar RESTful service
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class FooBarController : ControllerBase
{
    private readonly IAppBLL _bll;
    private readonly ILogger<FooBarController> _logger;
    private readonly Mapper _mapper;

    public FooBarController(IAppBLL bll, ILogger<FooBarController> logger, Mapper mapper)
    {
        _bll = bll;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets all FooBars
    /// </summary>
    /// <returns>All FooBars</returns>
    [Produces( "application/json" )]
    // GET: api/v1/FooBar
    [HttpGet]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.FooBar>>> GetFooBar()
    {
        var id =  Guid.Parse(this.User.Claims.First(i => i.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
        Console.WriteLine(id);
        
        var fooBarBll = (await _bll.FooBarService.GetAllAsync());
        var fooBar = fooBarBll.Select(bll => _mapper.FooBarMapper.Map(bll));    
        return Ok(fooBar);
    }
    
    /// <summary>
    /// Gets one FooBar based on its id
    /// </summary>
    /// <param name="id">FooBar id willing to get</param>
    /// <returns>Found FooBar</returns>
    /// <remarks>
    /// test
    /// </remarks>
    /// <responce code="404">User not found</responce>
    /// GET: api/v1/FooBar/5
    [HttpGet("{id}")]
    [SwaggerResponse(404, "Not asfdsafasdf found")]
    [Produces( "application/json" )]    
    public async Task<ActionResult<FooBar>> GetFooBar(Guid id)
    {
        var game = await _bll.FooBarService.FirstOrDefaultAsync(id);
        if (game == null)
        {
            return NotFound();
        }
        return Ok(_mapper.FooBarMapper.Map(game));
    }
    
    /// <summary>
    /// Modifies FooBar
    /// </summary>
    /// <param name="id">FooBar id willing to modify</param>
    /// <param name="fooBar">New representation of FooBar</param>
    /// <returns>Modified game</returns>
    // PUT: api/v1/FooBar/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [AllowAnonymous]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutFooBar(Guid id, App.Public.DTO.v1.FooBar fooBar)
    {
        if (id != fooBar.Id)
        {
            return BadRequest();
        }

        var fooBarBll = _mapper.FooBarMapper.Map(fooBar);
        if (fooBarBll == null)
        {
            return NotFound("incorrect base");
        }
        try
        {
            _bll.FooBarService.Update(fooBarBll);
            await _bll.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!FooBarExists(id))
            {
                return NotFound("something");
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }
    /// <summary>
    /// Creates FooBar
    /// </summary>
    /// <param name="fooBar">FooBar object willing to add</param>
    /// <returns>Created game</returns>
    // POST: api/Game
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    //[Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost]
    public async Task<ActionResult<FooBar>> PostFooBar(App.Public.DTO.v1.FooBar fooBar)
    {
        var fooBarBll = _mapper.FooBarMapper.Map(fooBar);
        if (fooBarBll == null)
        {
            return NotFound("incorrect base");
        }
        
        var result = _bll.FooBarService.Add(fooBarBll);
        await _bll.SaveChangesAsync();
        
        return Ok(_mapper.FooBarMapper.Map(result));
    }
    /// <summary>
    /// Deletes FooBar
    /// </summary>
    /// <param name="id">FooBar id willing to delete</param>
    /// <returns>Status Code</returns>
    // DELETE: api/v1/FooBar/5
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFooBar(Guid id)
    {
        var game = await _bll.FooBarService.FirstOrDefaultAsync(id);
        if (game == null)
        {
            return NotFound();
        }

        _bll.FooBarService.Remove(game);
        await _bll.SaveChangesAsync();

        return NoContent();
    }

    private bool FooBarExists(Guid id)
    {
        return _bll.FooBarService.Exists(id);
    }
}