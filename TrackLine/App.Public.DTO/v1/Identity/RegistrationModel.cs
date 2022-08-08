using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.Public.DTO.v1.Identity;

[DisplayName("Registration")]
public class RegistrationModel
{
    [Required]
    [StringLength(maximumLength:128, MinimumLength = 5, ErrorMessage = "Wrong length on email")]
    public string Email { get; set; } = default!;
    [Required]
    [StringLength(maximumLength:128, MinimumLength = 5, ErrorMessage = "Wrong length on username")]
    public string Username { get; set; } = default!;
    [Required]
    [StringLength(maximumLength:128, MinimumLength = 6, ErrorMessage = "Wrong length on password")]
    public string Password { get; set; } = default!;
}