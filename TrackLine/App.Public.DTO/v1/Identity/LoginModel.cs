using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.Public.DTO.v1.Identity;

[DisplayName("Login")]
public class LoginModel
{   
    [Required]
    [Display(Name = "Username or Email")]
    [StringLength(maximumLength:128, MinimumLength = 5, ErrorMessage = "Wrong length on email")]
    public string EmailOrUsername { get; set; } = default!;
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;

    [Display(Name = "Remember me?")] 
    public bool RememberMe { get; set; } = default!;
}

public enum LoginMethod
{
    Username,
    Email
}