using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.Public.DTO.v1.Identity;

[DisplayName("Login")]
public class LoginModel
{   
    [Required]
    [StringLength(maximumLength:128, MinimumLength = 5, ErrorMessage = "Wrong length on email")]
    public string Email { get; set; } = default!;
    [Required]
    public string Password { get; set; } = default!;
}