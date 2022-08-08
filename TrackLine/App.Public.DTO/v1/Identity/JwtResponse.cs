using System.ComponentModel;

namespace App.Public.DTO.v1.Identity;

/// <summary>
/// Token responce
/// </summary>
[DisplayName("JwtResponse")]
public class JwtResponse
{
    public string JWT { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
    public string Username { get; set; } = default!;
}