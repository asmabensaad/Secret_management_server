using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Auth.Models;
using Auth.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    public static User user = new User();
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;

    public AuthController(IConfiguration configuration, IUserService userService)
    {
        _configuration = configuration;
        _userService = userService;
    }

    [HttpGet, Authorize]
    public ActionResult<string> GetMe()
    {
        var userName = _userService.GetMyName();
        return userName;
        // return Ok(userName);
    }
/// <summary>
/// Register
/// </summary>
/// <param name="request"></param>
/// <returns></returns>
    [HttpPost("register")]
    public Task<ActionResult<User>> Register(UserRegistration request)
    {
        CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
        user.Username = request.Username;
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        return Task.FromResult<ActionResult<User>>(Ok(user));
    }
/// <summary>
/// Login
/// </summary>
/// <param name="request"></param>
/// <returns></returns>
    [HttpPost("login")]
    public Task<ActionResult<string>> Login(UserRegistration request)
    {
        if (user.Username != request.Username)
        {
            return Task.FromResult<ActionResult<string>>(BadRequest("user not found."));
        }

        if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
        {
            return Task.FromResult<ActionResult<string>>(BadRequest(("wrong  password")));
        }

        string token = CreateToken(user);
        var refreshToken = GenerateRefreshToken();
        SetRefreshToken(refreshToken);
        return Task.FromResult<ActionResult<string>>(Ok(token));
    }
/// <summary>
/// RefreshToken
/// </summary>
/// <returns></returns>
    [HttpPost("refresh-token")]
    public Task<ActionResult<string>> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (!user.RefreshToken.Equals(refreshToken))
        {
            return Task.FromResult<ActionResult<string>>(Unauthorized("Invalid Refresh Token"));
        }
        else if (user.TokenExpires < DateTime.Now)
        {
            return Task.FromResult<ActionResult<string>>(Unauthorized("Token expired."));
        }

        string token = CreateToken(user);
        var newRefreshToken = GenerateRefreshToken();
        SetRefreshToken(newRefreshToken);
        return Task.FromResult<ActionResult<string>>(Ok(token));
    }
/// <summary>
/// GenerateRefreshToken
/// </summary>
/// <returns></returns>
    private RefreshToken GenerateRefreshToken()
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.Now.AddDays(7),
            created = DateTime.Now
        };
        return refreshToken;
    }
/// <summary>
/// SetRefreshToken
/// </summary>
/// <param name="newRefreshToken"></param>
    private void SetRefreshToken(RefreshToken newRefreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = newRefreshToken.Expires
        };
        Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
        user.RefreshToken = newRefreshToken.Token;
        user.TokenCreated = newRefreshToken.created;
        user.TokenExpires = newRefreshToken.Expires;
    }
/// <summary>
/// CreateToken
/// </summary>
/// <param name="user"></param>
/// <returns></returns>
    private string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, "Admin")
        };
        var key = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            notBefore: DateTime.Now,
            signingCredentials: creds
        );


        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }
/// <summary>
/// CreatePasswordHash
/// </summary>
/// <param name="password"></param>
/// <param name="passwordHash"></param>
/// <param name="passwordSalt"></param>
    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

/// <summary>
/// VerifyPasswordHash
/// </summary>
/// <param name="password"></param>
/// <param name="passworHash"></param>
/// <param name="passwordSalt"></param>
/// <returns></returns>
    private bool VerifyPasswordHash(string password, byte[] passworHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passworHash);
        }
    }
}