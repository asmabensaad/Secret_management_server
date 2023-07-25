using Microsoft.AspNetCore.Identity;

namespace Auth2;

public class ApplicationUser :IdentityUser
{
    public string? RfereshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}