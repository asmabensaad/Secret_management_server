using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

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
    [JsonProperty("email")]
    public string Email { get; set; }

    /// <summary>
    /// Password is required 
    /// </summary>
    [Required(ErrorMessage = "Password is required")]
    [JsonProperty("password")]
    public string Password { get; set; }
}