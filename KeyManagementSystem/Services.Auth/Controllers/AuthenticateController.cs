using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Auth2;
using DataAccess.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Services.Auth.Controllers;

//TODO: Follow naming convention
//TODO: Remove commented code
//TODO: Fix spelling
[Route("api/[controller]")]
[ApiController]
public class AuthenticateController : ControllerBase
{

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor httpC;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthenticateController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
        IConfiguration configuration, IHttpContextAccessor _httpC,SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        httpC = _httpC;
        _signInManager = signInManager;

    }

    /// <summary>
    /// Authentication
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.email);
        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };


            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = CreateToken(authClaims);
            var refreshToken = GenerateRefreshToken();
            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);
            user.RfereshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);
            await _userManager.UpdateAsync(user);
            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Expiration = token.ValidTo
            });

        }

        return Unauthorized();
    }

    /// <summary>
    /// registration
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var userExist = await _userManager.FindByEmailAsync(model.Email);
        if (userExist != null)

            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Error", Message = "User already exist!" });
        ApplicationUser user = new()
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Username,
            PhoneNumber = model.contactNumber,

        };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response
                    { Status = "Error", Message = "User creation failed! Please check user details and try again." });
        if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
        if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
        if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
        {
            await _userManager.AddToRoleAsync(user, UserRoles.Admin);
        }

        if (await _roleManager.RoleExistsAsync(UserRoles.User))
        {
            await _userManager.AddToRoleAsync(user, UserRoles.User);
        }

        return Ok(new Response { Status = "Success", Message = "User created successfully!" });


    }

    /// <summary>
    /// Generate the refresh Token
    /// </summary>
    /// <returns></returns>

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);

    }

    /// <summary>
    /// create Token 
    /// </summary>
    /// <param name="authClaims"></param>
    /// <returns></returns>


    private JwtSecurityToken CreateToken(List<Claim> authClaims)
    {
        var authSiginKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
        _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);
        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],

            expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSiginKey, SecurityAlgorithms.HmacSha256)
        );
        return token;

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tokenModel"></param>
    /// <returns></returns>

    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshToken(TokenModel? tokenModel)
    {
        if (tokenModel is null)
        {
            return BadRequest("Invalid client request");
        }

        string? accessToken = tokenModel.AccessToken;
        string? refreshToken = tokenModel.RefreshToken;


        var principal = GetPrincipalFromExpiredToken(accessToken);
        if (principal == null)
        {
            return BadRequest("Invalid access token or refresh token");

        }
        string username = principal.Identity.Name;

        var user = await _userManager.FindByNameAsync(username);
        if (user == null || user.RfereshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            return BadRequest("Invalid access token or refresh token");
        }

        var newAcessToken = CreateToken(principal.Claims.ToList());
        var newRefreshToken = GenerateRefreshToken();
        user.RfereshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);
        return new ObjectResult(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(newAcessToken),
            refreshToken = newRefreshToken
        });

    }

    /// <summary>
    /// revoke a token from a specified user
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>

    [Authorize]
    [HttpPost]
    [Route("revoke/{username}")]
    public async Task<IActionResult> Revoke(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return BadRequest("Invalid user name");

        user.RfereshToken = null;
        await _userManager.UpdateAsync(user);
        return NoContent();
    }

    /// <summary>
    /// revoke all token from all user
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("revoke-all")]
    public async Task<IActionResult> RevokeAll()
    {
        var users = _userManager.Users.ToList();
        foreach (var user in users)
        {
            user.RfereshToken = null;
            await _userManager.UpdateAsync(user);

        }

        return NoContent();
    }


    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
            ValidateLifetime = false,





        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityException("invalid token!");
        return principal;
    }

    [HttpGet]
    [Route("GetAllUsers")]
    public async Task<IActionResult> GetUsers()
    {
        var allUsers = await _userManager.Users.ToListAsync();
        var userInformationList = allUsers.Select(u => new RegisterModel()
        {

            Username = u.UserName,
            Email = u.Email,
            contactNumber = u.PhoneNumber


        }).ToList();
        return Ok(userInformationList);
    }

    [Authorize]
    [HttpGet]
    [Route("getCurrentUser")]
    public async Task<IActionResult> GetUser()
    {


        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                return Ok(new
                {
                    userId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email
                });
            }

            
            // var user = await _userManager.GetUserAsync(HttpContext.User);
            // if (user == null)
            // {
            //     return Ok(new { Message = "User data not available" });
            // }
            //
            // return Ok(new { UserName = user.UserName });
            //
            //
        }
        return BadRequest("user not authenticated");

    }
    [Authorize]
    [HttpGet]
  
        int GetLoggedUserId()
        {
            if (!User.Identity.IsAuthenticated)
                throw new AuthenticationException();

            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            return int.Parse(userId);
        }
        
    

   [Authorize]
    [HttpPost]
    [Route("logout")]
    public async Task<IActionResult> Logout()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user != null)
        {
            user.RfereshToken = null;
            await _userManager.UpdateAsync(user);
        }

        await _signInManager.SignOutAsync();
        return Ok(new { message = "logout successful" });
    }
    
}

