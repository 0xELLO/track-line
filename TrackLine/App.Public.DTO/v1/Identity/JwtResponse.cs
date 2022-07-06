using System.ComponentModel;

namespace App.Public.DTO.v1.Identity;

/// <summary>
/// Token responce
/// </summary>
[DisplayName("JwtResponse")]
public class JwtResponse
{
    public string Token { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
    public string Email { get; set; }= default!;
}