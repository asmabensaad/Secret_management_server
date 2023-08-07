using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models.AuthService;

/// <summary>
/// Login Model 
/// </summary>
public class LoginModel
{
    /// <summary>
    /// Email is required
    /// </summary>
    [Required(ErrorMessage = "User Email is required")]
    public string Email { get; set; }

    /// <summary>
    /// Password is required 
    /// </summary>
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
}