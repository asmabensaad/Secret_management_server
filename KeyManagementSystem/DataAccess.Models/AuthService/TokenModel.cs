namespace DataAccess.Models.AuthService;

/// <summary>
/// Token Model
/// </summary>
public class TokenModel
{
    /// <summary>
    /// Access Token
    /// </summary>
    public string AccessToken { get; set; }

    /// <summary>
    /// Refresh Token
    /// </summary>
    public string RefreshToken { get; set; }
}