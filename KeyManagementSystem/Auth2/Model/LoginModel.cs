using System.ComponentModel.DataAnnotations;

namespace Auth2;

public class LoginModel
{
    [Required(ErrorMessage = "User Email is required")]
    public string? email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
}