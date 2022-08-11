using System.Collections;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using App.Contracts.BLL;
using App.DAL.EF;
using App.Domain.Identity;
using App.Public.DTO.v1.Helpers;
using App.Public.DTO.v1.Identity;
using Base.Common;
using Base.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using WebApp.ApiControllers.Helpers;

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
    // TODO change email method
    // TODO remove context!!! + RefreshToken Service!!1 finished?

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
        var errorBuilder = new ErrorBuilder();
        var loginMethod = LoginMethod.Email;
        
        if (!loginModelData.EmailOrUsername.Contains("@")) loginMethod = LoginMethod.Username; 

        var appUser = await _userManager.FindByEmailAsync(loginModelData.EmailOrUsername);
        
        if (loginMethod == LoginMethod.Username)
        {
            appUser = await _userManager.FindByNameAsync(loginModelData.EmailOrUsername);
        }
        
        if (appUser == null)
        {
            var identityError = new IdentityError() 
            {
                Code = "InvalidCredentials",
                Description = $"Invalid credentials for user '{loginModelData.EmailOrUsername}'."
            };
            
            var error = errorBuilder.ErrorResponse(Activity.Current?.Id ?? HttpContext.TraceIdentifier, new List<IdentityError>() { identityError });
            return BadRequest(error);
        }
        
        // user validation
        var userValidator = new UserValidator<AppUser>();
        var userValidatorRes = await userValidator.ValidateAsync(_userManager, appUser);
        
        if (!userValidatorRes.Succeeded)
        {
            await Task.Delay(_rnd.Next(100, 1000));
            var identityError = new IdentityError() 
            {
                Code = "InvalidCredentials",
                Description = $"Invalid credentials for user '{loginModelData.EmailOrUsername}'."
            };
            
            var error = errorBuilder.ErrorResponse(Activity.Current?.Id ?? HttpContext.TraceIdentifier, new List<IdentityError>() { identityError });
            return BadRequest(error);
        }
        
        // password validation
        var passwordValidationRes = await _signInManager.PasswordSignInAsync(appUser, loginModelData.Password, loginModelData.RememberMe, true);
        if (!passwordValidationRes.Succeeded)
        {
            await Task.Delay(_rnd.Next(100, 1000));
            var identityError = new IdentityError() 
            {
                Code = "InvalidCredentials",
                Description = $"Invalid credentials for user '{loginModelData.EmailOrUsername}'."
            };
            if (passwordValidationRes.IsLockedOut)
            {
                // TODO Make the error more user-friendly. Somehow get the time when the lock out is finished?
                identityError = new IdentityError() 
                {
                    Code = "UserLockedOut",
                    Description = $"User '{loginModelData.EmailOrUsername}' is locked out. Please try again later."
                };
            }
            
            var error = errorBuilder.ErrorResponse(Activity.Current?.Id ?? HttpContext.TraceIdentifier, new List<IdentityError>() { identityError });
            return BadRequest(error);
        }

        // get claims based user
        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        if (claimsPrincipal == null)
        {
            await Task.Delay(_rnd.Next(100, 1000));
            var identityError = new IdentityError() 
            {
                Code = "InvalidCredentials",
                Description = $"Invalid credentials for user '{loginModelData.EmailOrUsername}'."
            };
            
            var error = errorBuilder.ErrorResponse(Activity.Current?.Id ?? HttpContext.TraceIdentifier, new List<IdentityError>() { identityError });
            return BadRequest(error);
        }
        
        // generate jwt
        var jwt = IdentityExtensions.GenerateJwt(
            claimsPrincipal.Claims,
            _configuration["JWT:Key"],
            _configuration["JWT:Issuer"],
            _configuration["JWT:Issuer"],
            DateTime.Now.AddMinutes(_configuration.GetValue<int>("JWT:ExpireInMinutes"))
            );
        
        // new refresh token load
        appUser.RefreshTokens = (await _bll.RefreshTokenService
            .GetValidRefreshTokensByUserIdAsync(appUser.Id))
            .Select(x => new RefreshToken
            {
                Id = x.Id,
                Token = x.Token,
                ExpirationTime = x.ExpirationTime,
                PreviousToken = x.PreviousToken,
                PreviousExpirationTime = x.PreviousExpirationTime,
                AppUserId = x.AppUserId,
            }).ToList();

        
        var refreshToken = await _bll.RefreshTokenService.GenerateRefreshToken(appUser.Id);
        await _bll.SaveChangesAsync();

        var res = new JwtResponse()
        {
            JWT = jwt,
            RefreshToken = refreshToken.Token,
            Username = appUser.UserName
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
        var errorBuilder = new ErrorBuilder();

        var refreshToken = new RefreshToken();
        var appUser = new AppUser()
        {
            UserName = registrationData.Username,
            Email = registrationData.Email, 
            RefreshTokens = new List<RefreshToken>()
            {
                refreshToken
            }
        };
        
        // user validation
        var userValidator = new UserValidator<AppUser>();
        var userValidatorRes = await userValidator.ValidateAsync(_userManager, appUser);
        if (!userValidatorRes.Succeeded)
        {
            var error = errorBuilder.ErrorResponse(Activity.Current?.Id ?? HttpContext.TraceIdentifier, userValidatorRes.Errors);
            return BadRequest(error);
        }
        
        // custom email verification
        var trimmedEmail = appUser.Email.Trim();
        if (trimmedEmail.EndsWith("."))
        {
            var identityError = new IdentityError() 
            {
                Code = "InvalidEmail",
                Description = $"Email '{appUser.Email}' is invalid."
            };
            
            var error = errorBuilder.ErrorResponse(Activity.Current?.Id ?? HttpContext.TraceIdentifier, new List<IdentityError>() { identityError });
            return BadRequest(error);
        }
        
        // password validation
        var passwordValidator = new PasswordValidator<AppUser>();
        var passwordValidationRes = await passwordValidator.ValidateAsync(_userManager, appUser, registrationData.Password);
        if (!passwordValidationRes.Succeeded)
        {
            var error = errorBuilder.ErrorResponse(Activity.Current?.Id ?? HttpContext.TraceIdentifier, passwordValidationRes.Errors);
            return BadRequest(error);
        }

        // create user (system will do it)
        var createUserRes = await _userManager.CreateAsync(appUser, registrationData.Password);
        if (!createUserRes.Succeeded)
        {
            return BadRequest(createUserRes.Errors.FirstOrDefault());
        }
        
        // add role user (system will do it)
        var addToRoleRes = await _userManager.AddToRoleAsync(appUser, AppUserRoles.User.ToString());
        if (!addToRoleRes.Succeeded)
        {
            return BadRequest(addToRoleRes.Errors.FirstOrDefault());
        }
        
        // get full user from system with fixed data
        appUser = await _userManager.FindByEmailAsync(appUser.Email);
        if (appUser == null)
        {
            var identityError = new IdentityError() 
            {
                Code = "AccountNotFound",
                Description = $"User with email '{registrationData.Email}' is not found after the registration."
            };
            
            var error = errorBuilder.ErrorResponse(Activity.Current?.Id ?? HttpContext.TraceIdentifier, new List<IdentityError>() { identityError });
            return BadRequest(error);
        }

        // get claims based user
        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        if (claimsPrincipal == null)
        {
            var identityError = new IdentityError() 
            {
                Code = "ClaimsPrincipalError",
                Description = $"Could not get claimsPrincipal for user '{registrationData.Email}'."
            };
            
            var error = errorBuilder.ErrorResponse(Activity.Current?.Id ?? HttpContext.TraceIdentifier, new List<IdentityError>() { identityError });
            return BadRequest(error);
        }
        
        // generate Default lists
        var headLists = await _bll.HeadListService.GenerateDefaultHeadLists(appUser.Id);
        await _bll.SaveChangesAsync();
        foreach (var list in headLists)
        {
            await _bll.SubListService.GenerateDefaultSubLists(list.Id);
        }

        await _bll.SaveChangesAsync();
        
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
            JWT = jwt,
            RefreshToken = refreshToken.Token,
            Username = appUser.UserName
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
        var errorBuilder = new ErrorBuilder();

        // get info from JWT
        //JwtSecurityToken jwtToken;
        try
        {
            //jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(refreshTokenModel.Jwt);
            //if (jwtToken == null)
            if (refreshTokenModel.Jwt == null)
            {
                var identityError = new IdentityError() 
                {
                    Code = "RefreshTokenError",
                    Description = "No token found, please try again."
                };
            
                var error = errorBuilder.ErrorResponse(Activity.Current?.Id ?? HttpContext.TraceIdentifier, new List<IdentityError>() { identityError });
                return BadRequest(error);
            }
        }
        catch (Exception e)
        {
            // TODO should we parse e.Message in prod?
            var identityError = new IdentityError() 
            {
                Code = "RefreshTokenError",
                Description = $"Could not parse the token '{e.Message}'." 
            };
            
            var error = errorBuilder.ErrorResponse(Activity.Current?.Id ?? HttpContext.TraceIdentifier, new List<IdentityError>() { identityError });
            return BadRequest(error);
        }
        
        // validate token siganture
        string userEmail;
        var token = refreshTokenModel.Jwt;
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWT:Key"));
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken) validatedToken;
            userEmail = jwtToken.Claims.First(x => x.Type == ClaimTypes.Email).Value;
        }
        catch
        {
            var identityError = new IdentityError() 
            {
                Code = "RefreshTokenError",
                Description = "Something went wrong while Refresh Token validation." 
            };
            
            var error = errorBuilder.ErrorResponse(Activity.Current?.Id ?? HttpContext.TraceIdentifier, new List<IdentityError>() { identityError });
            return BadRequest(error); 
        }
        
        // verify that userEmail has some data
        if (userEmail == null || userEmail == "")
        {
            var identityError = new IdentityError() 
            {
                Code = "RefreshTokenError",
                Description = "Something went wrong while Refresh Token validation." 
            };
            
            var error = errorBuilder.ErrorResponse(Activity.Current?.Id ?? HttpContext.TraceIdentifier, new List<IdentityError>() { identityError });
            return BadRequest(error);
        }
        
        // get user and tokens
        var appUser = await _userManager.FindByEmailAsync(userEmail);
        if (appUser == null)
        {
            var identityError = new IdentityError() 
            {
                Code = "RefreshTokenError",
                Description = "Something went wrong while Refresh Token validation." 
            };
            
            var error = errorBuilder.ErrorResponse(Activity.Current?.Id ?? HttpContext.TraceIdentifier, new List<IdentityError>() { identityError });
            return BadRequest(error);
        }
        
        // new load and compare refresh tokens
        appUser.RefreshTokens = (await _bll.RefreshTokenService.GetValidRefreshTokensByUserIdAsync(appUser.Id,
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
            var identityError = new IdentityError() 
            {
                Code = "RefreshTokenError",
                Description = "RefreshTokens collection is null." 
            };
            
            var error = errorBuilder.ErrorResponse(Activity.Current?.Id ?? HttpContext.TraceIdentifier, new List<IdentityError>() { identityError });
            return BadRequest(error);
        }
        
        if (appUser.RefreshTokens.Count == 0)
        {
            var identityError = new IdentityError() 
            {
                Code = "RefreshTokenError",
                Description = "No valid refresh tokens found, collection in empty." 
            };
            
            var error = errorBuilder.ErrorResponse(Activity.Current?.Id ?? HttpContext.TraceIdentifier, new List<IdentityError>() { identityError });
            return BadRequest(error);
        }
        
        if (appUser.RefreshTokens.Count != 1)
        {
            var identityError = new IdentityError() 
            {
                Code = "RefreshTokenError",
                Description = "More then one valid refresh token found." 
            };
            
            var error = errorBuilder.ErrorResponse(Activity.Current?.Id ?? HttpContext.TraceIdentifier, new List<IdentityError>() { identityError });
            return BadRequest(error);
        }

        // get claims based user
        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        if (claimsPrincipal == null)
        {
            var identityError = new IdentityError() 
            {
                Code = "ClaimsPrincipalError",
                Description = $"Could not get claimsPrincipal for user {userEmail}." 
            };
            
            var error = errorBuilder.ErrorResponse(Activity.Current?.Id ?? HttpContext.TraceIdentifier, new List<IdentityError>() { identityError });
            return BadRequest(error);
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
            JWT = jwt,
            RefreshToken = refreshToken.Token,
            Username = appUser.UserName
        };
        return Ok(res);
    }
}