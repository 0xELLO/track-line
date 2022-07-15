using System.Data;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using App.Contracts.BLL;
using App.DAL.EF;
using App.Domain.Identity;
using App.Public.DTO.v1.Helpers;
using App.Public.DTO.v1.Identity;
using Base.Common;
using Base.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApp.ApiControllers.Identity;
/// <summary>
/// Represents logging RESTful service.
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/identity/[controller]/[action]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<AccountController> _logger;
    private readonly IConfiguration _configuration;
    private readonly Random _rnd = new Random();
    private readonly AppDbContext _context;
    private readonly IAppBLL _bll;

    public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager,
        ILogger<AccountController> logger, IConfiguration configuration, AppDbContext context, IAppBLL bll)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
        _configuration = configuration;
        _context = context;
        _bll = bll;
    }
    
    // TODO change password method
    // TODO change username?? method (firstly add username)
    // TODO change email method
    // TODO remove context!!! + RefreshToken Service!!1 finished?
    // TODO error descriptions
    // TODO Api error description over standard errors (do i need traceId Mark prochitai pro traceId)

    /// <summary>
    /// AppUser authentication 
    /// </summary>
    /// <param name="loginModelData">Login data: username, password</param>
    /// <returns>JWT token, refresh token</returns>
    [HttpPost]
    [SwaggerResponse(404, "Validation error", typeof(RestApiErrorResponse))]
    [SwaggerResponse(200, "Authentication successful")]
    public async Task<ActionResult<JwtResponse>> LogIn([FromBody]LoginModel loginModelData)
    {
        // standard password/email validation error (reusable)
        var validationError = new RestApiErrorResponse
        {
            Type = ApiErrorType.BadRequestType,
            Title = ApiErrorTitle.ValidationErrorTitle,
            Status = (int) HttpStatusCode.NotFound,
            TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
            Errors =
            {
                [ApiErrorSource.EmailOrPasswordSource] = new List<string>()
                {
                    ApiErrorDescription.EmailOrPasswordProblemDescription
                }
            }
        };

        // verify username
        var appUser = await _userManager.FindByEmailAsync(loginModelData.Email);
        if (appUser == null)
        {
            _logger.LogWarning("WebApi login failed email {} not found", loginModelData.Email);
            await Task.Delay(_rnd.Next(100, 1000));
            return NotFound(validationError);
        }
        
        // verify username and password
        var result = await _signInManager.CheckPasswordSignInAsync(appUser, loginModelData.Password, false);
        if (!result.Succeeded)
        {
            _logger.LogWarning("WebApi login failed password for email {} ", loginModelData.Email);
            await Task.Delay(_rnd.Next(100, 1000));
            return NotFound(validationError);
        } 
        
        // get claims based user
        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        if (claimsPrincipal == null)
        {
            _logger.LogWarning("Could not get claimsPrincipal for user {}", loginModelData.Email);
            await Task.Delay(_rnd.Next(100, 1000));
            return NotFound(validationError);
        }
        
        // generate jwt
        var jwt = IdentityExtensions.GenerateJwt(
            claimsPrincipal.Claims,
            _configuration["JWT:Key"],
            _configuration["JWT:Issuer"],
            _configuration["JWT:Issuer"],
            DateTime.Now.AddMinutes(_configuration.GetValue<int>("JWT:ExpireInMinutes"))
            );

        // old refresh token load
        /*
        appUser.RefreshTokens = await _context
            .Entry(appUser)
            .Collection(a => a.RefreshTokens!)
            .Query()
            .Where(t => t.AppUserId == appUser.Id)
            .ToListAsync();
            
        foreach (var appUserRefreshToken in appUser.RefreshTokens)
        {
            if (appUserRefreshToken.ExpirationTime < DateTime.UtcNow
                && appUserRefreshToken.PreviousExpirationTime < DateTime.UtcNow)
            {
                _context.RefreshToken.Remove(appUserRefreshToken);
            }
        }
        */
        
        // new refresh token load
        appUser.RefreshTokens = (await _bll.RefreshTokenService
            .GetValidRefreshTokensByUserIdAsync(appUser.Id.ToString()))
            .Select(x => new RefreshToken
            {
                Id = x.Id,
                Token = x.Token,
                ExpirationTime = x.ExpirationTime,
                PreviousToken = x.PreviousToken,
                PreviousExpirationTime = x.PreviousExpirationTime,
                AppUserId = x.AppUserId,
            }).ToList();


        var refreshToken = new App.BLL.DTO.Identity.RefreshTokenDTO();
        refreshToken.AppUserId = appUser.Id;
        await _bll.SaveChangesAsync();

        var res = new JwtResponse()
        {
            Token = jwt,
            RefreshToken = refreshToken.Token,
            Email = appUser.Email
        };
        return Ok(res);
    }
    
    /// <summary>
    /// Registers user into the system.
    /// </summary>
    /// <param name="registrationData">Registration data: login, password</param>
    /// <returns>JWT token, refresh token</returns>
    [HttpPost]
    [SwaggerResponse(404, "Validation error", typeof(RestApiErrorResponse))]
    [SwaggerResponse(400, "Validation error", typeof(RestApiErrorResponse))]
    [SwaggerResponse(200, "Authentication successful")]
    public async Task<ActionResult<JwtResponse>> Register(RegistrationModel registrationData)
    {
        // verify user
        var appUser = await _userManager.FindByEmailAsync(registrationData.Email);
        if (appUser != null)
        {
            _logger.LogWarning("WebApi login failed password for email {} ", registrationData.Email);
            var error = new RestApiErrorResponse
            {
                Type = ApiErrorType.BadRequestType,
                Title = ApiErrorTitle.EmailValidationErrorTitle,
                Status = (int) HttpStatusCode.BadRequest,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    [ApiErrorSource.EmailSource] = new List<string>()
                    {
                        ApiErrorDescription.EmailAlreadyRegisteredDescription
                    }
                }
            };
            return BadRequest(error);
        }

        var refreshToken = new RefreshToken();
        appUser = new AppUser()
        {
            UserName = registrationData.Email,
            Email = registrationData.Email, 
            RefreshTokens = new List<RefreshToken>()
            {
                refreshToken
            }
        };
        
        // validate
        var passwordValidator = new PasswordValidator<AppUser>();
        var passwordValidationRes = await passwordValidator.ValidateAsync(_userManager, appUser, registrationData.Password);
        if (!passwordValidationRes.Succeeded)
        {
            return BadRequest(new RestApiErrorResponse
            {
                Type = ApiErrorType.BadRequestType,
                Title = ApiErrorTitle.PasswordValidationErrorTitle,
                Status = (int) HttpStatusCode.BadRequest,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    [ApiErrorSource.PasswordSource] = passwordValidationRes.Errors.Select(e => e.Description).ToList()
                }
            });
        }
        
        var badEmailError = new RestApiErrorResponse
        {
            Type = ApiErrorType.BadRequestType,
            Title = ApiErrorTitle.EmailValidationErrorTitle,
            Status = (int) HttpStatusCode.BadRequest,
            TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
            Errors =
            {
               [ApiErrorSource.EmailSource] = new List<string>
                {
                    ApiErrorDescription.EmailValidationErrorDescription
                }
            }
        };
        
        var trimmedEmail = appUser.Email.Trim();
        try {
            var addr = new System.Net.Mail.MailAddress(appUser.Email);
            if (addr.Address != trimmedEmail || trimmedEmail.EndsWith("."))
            {
                return BadRequest(badEmailError);
            }
        }
        catch {
            return BadRequest(badEmailError);
        }
        
        // create user (system will do it)
        var result = await _userManager.CreateAsync(appUser, registrationData.Password);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.FirstOrDefault());
        }
        
        // add role user (system will do it)
        var result2 = await _userManager.AddToRoleAsync(appUser, AppUserRoles.User.ToString());
        if (!result2.Succeeded)
        {
            return BadRequest(result2.Errors.FirstOrDefault());
        }
        
        // get full user from system with fixed ddata
        appUser = await _userManager.FindByEmailAsync(appUser.Email);
        if (appUser == null)
        {
            _logger.LogWarning("User with email {} is not found after the registration", registrationData.Email);
            return BadRequest($"User with email { registrationData.Email } is not found after the registration");
        }

        // get claims based user
        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        if (claimsPrincipal == null)
        {
            _logger.LogWarning("Could not get claimsPrincipal for user {}", registrationData.Email);
            return NotFound("User/Password problem");
        }
        
        // generate jwt
        var jwt = IdentityExtensions.GenerateJwt(
            claimsPrincipal.Claims,
            _configuration["JWT:Key"],
            _configuration["JWT:Issuer"],
            _configuration["JWT:Issuer"],
            DateTime.Now.AddMinutes(_configuration.GetValue<int>("JWT:ExpireInMinutes"))
        );
        var res = new JwtResponse()
        {
            Token = jwt,
            RefreshToken = refreshToken.Token,
            Email = appUser.Email
        };
        return Ok(res);
    }
    
    /// <summary>
    /// Refreshes JWT token
    /// </summary>
    /// <param name="refreshTokenModel">JWT token</param>
    /// <returns>New JWT token</returns>
    /// <responce code="200">User authentication successful</responce>
    [HttpPost]
    [ProducesResponseType(200)]
    public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenModel refreshTokenModel)
    {
        // get info from JWT
        JwtSecurityToken jwtToken;
        try
        {
            jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(refreshTokenModel.Jwt);
            if (jwtToken == null)
            {
                return BadRequest("No token");
            }
        }
        catch (Exception e)
        {
            return BadRequest($"Can't parse the token {e.Message}");
        }
        
        // TODO: validate token signature
        // https://stackoverflow.com/questions/49407749/jwt-token-validation-in-asp-net
        // validate token siganture
        var userEmail = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        if (userEmail == null)
        {
            return BadRequest("No email");
        }
        
        // get user and tokens
        var appUser = await _userManager.FindByEmailAsync(userEmail);
        if (appUser == null)
        {
            return BadRequest($"User with email {userEmail} not found");
        }

        // old load and compare refresh tokens
        /*
        await _context.Entry(appUser).Collection(u => u.RefreshTokens!)
            .Query()
            .Where(x =>
                (x.Token == refreshTokenModel.RefreshToken && x.ExpirationTime > DateTime.UtcNow) ||
                (x.PreviousToken == refreshTokenModel.RefreshToken && x.PreviousExpirationTime > DateTime.UtcNow))
            .ToListAsync();
        */
        
        // new load and compare refresh tokens
        appUser.RefreshTokens = (await _bll.RefreshTokenService.GetValidRefreshTokensByUserIdAsync(appUser.Id.ToString(),
            refreshTokenModel.RefreshToken)).Select(x => new RefreshToken
        {
            Id = x.Id,
            Token = x.Token,
            ExpirationTime = x.ExpirationTime,
            PreviousToken = x.PreviousToken,
            PreviousExpirationTime = x.PreviousExpirationTime,
            AppUserId = x.AppUserId,
        }).ToList();
        
        
        if (appUser.RefreshTokens == null)
        {
            return Problem("RefreshTokens collection is null");
        }
        
        if (appUser.RefreshTokens.Count == 0)
        {
            return Problem("No valid refresh tokens found, collection in empty");
        }
        
        if (appUser.RefreshTokens.Count != 1)
        {
            return Problem("More then one valid refresh token found");
        }
        
        // generate new jwt
        
        // get claims based user
        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        if (claimsPrincipal == null)
        {
            _logger.LogWarning("Could not get claimsPrincipal for user {}", userEmail);
            return NotFound("User/Password problem");
        }
        
        // generate jwt
        var jwt = IdentityExtensions.GenerateJwt(
            claimsPrincipal.Claims,
            _configuration["JWT:Key"],
            _configuration["JWT:Issuer"],
            _configuration["JWT:Issuer"],
            DateTime.Now.AddMinutes(_configuration.GetValue<int>("JWT:ExpireInMinutes"))
        );
        
        // make new refresh token, onsolate old ones
        var refreshToken = appUser.RefreshTokens.First();
        if (refreshToken.Token == refreshTokenModel.RefreshToken)
        {
            refreshToken.PreviousToken = refreshToken.Token;
            refreshToken.PreviousExpirationTime = DateTime.UtcNow.AddMinutes(1);

            refreshToken.Token = Guid.NewGuid().ToString();
            refreshToken.ExpirationTime = DateTime.UtcNow.AddDays(7);

            await _context.SaveChangesAsync();
        }
  
        
        var res = new JwtResponse()
        {
            Token = jwt,
            RefreshToken = refreshToken.Token,
            Email = appUser.Email
        };
        return Ok(res);
    }
}