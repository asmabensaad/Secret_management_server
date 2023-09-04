using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models.AuthService;

public class ForgotPasswordModel
{
    [Required]
    [EmailAddress]
    public string email { get; set; }
}