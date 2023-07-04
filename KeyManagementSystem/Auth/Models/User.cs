namespace Auth.Models;

public class User
{
    public string Username { get; set; } =String.Empty;
    public byte[] PasswordHash { get; set; } 
    public byte[]  PasswordSalt { get; set; }
    public String RefreshToken { get; set; } = string.Empty;
    public DateTime TokenCreated { get; set; }
    public DateTime TokenExpires { get; set; }
}