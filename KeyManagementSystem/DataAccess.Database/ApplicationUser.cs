using Microsoft.AspNetCore.Identity;

namespace DataAccess.Database;

public class ApplicationUser :IdentityUser
{
    public string? RfereshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}