namespace Auth.Models;

public class RefreshToken
{
    public string Token { get; set; } = string.Empty;
    public DateTime created { get; set; } =DateTime.Now;
    public DateTime Expires { get; set; }
}