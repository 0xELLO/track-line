using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.Public.DTO.v1.Identity;

/// <summary>
/// Represents refresh token input
/// User willing to refresh their token sends this model 
/// </summary>
[DisplayName("RefreshToken")]
public class RefreshTokenModel
{
    [Required]
    public string Jwt { get; set; } = default!;
    [Required]
    public string RefreshToken { get; set; } = default!;
}