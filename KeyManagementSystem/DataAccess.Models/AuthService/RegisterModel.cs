using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models.AuthService;

/// <summary>
/// Register Model
/// </summary>
public class RegisterModel
{
    /// <summary>
    /// auto increment field Id
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; init; }

    /// <summary>
    /// UserName
    /// </summary>
    [Required(ErrorMessage = "User Name is required")]
    public string Username { get; init; }

    /// <summary>
    /// Email
    /// </summary>
    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; init; }

    /// <summary>
    /// Password 
    /// </summary>
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }

    /// <summary>
    /// Phone Number
    /// </summary>
    public string ContactNumber { get; init; }
}