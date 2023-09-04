using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Core.Security.Authentication;

public class KmsTokenHandler
{
    private readonly IConfiguration _configuration;

    public KmsTokenHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// create Token 
    /// </summary>
    /// <param name="authClaims"></param>
    /// <returns></returns>
    public string CreateToken(IEnumerable<Claim> authClaims) // TODO: Use username and password
    {
        var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
        _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out var tokenValidityInMinutes);
        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256)
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// Generate the refresh Token
    /// </summary>
    /// <returns></returns>
    public string CreateRefreshToken(string userId)
    {
        var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            expires: DateTime.Now.AddDays(7),
            claims: new[]
            {
                new Claim("typ", "refresh"),
                new Claim("sub", userId)
            },
            signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public TokenValidationParameters GetValidationParameters(bool isRefresh)
    {
        var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

        if (isRefresh)
        {
            return new TokenValidationParameters
            {
                ValidateLifetime = false,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidIssuer = _configuration["JWT:ValidIssuer"],
                IssuerSigningKey = authSigninKey
            };
        }

        return new TokenValidationParameters
        {
            ValidateLifetime = false,
            ValidateAudience = true,
            ValidAudience = _configuration["JWT:ValidAudience"],
            ValidateIssuer = true,
            ValidIssuer = _configuration["JWT:ValidIssuer"],
            IssuerSigningKey = authSigninKey
        };
    }

    public bool IsValidateToken(string authToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = GetValidationParameters(true);

        _ = tokenHandler.ValidateToken(authToken, validationParameters, out _);
        return true;
    }
}