using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Core.Security.Authentication;
using DataAccess.Database;
using DataAccess.Models.Api;
using DataAccess.Models.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Services.Auth.Repository;

namespace Services.Auth.Controllers;

//TODO: Follow naming convention
//TODO: Remove commented code
//TODO: Fix spelling
[Route("api/[controller]")]
[ApiController]
public class AuthenticateController : ControllerBase
{   private readonly IEmailSender _emailSender;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly KmsTokenHandler _tokenHandler;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthenticateController(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration, IEmailSender emailSender,SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _emailSender = emailSender;
        _signInManager = signInManager;
        _tokenHandler = new KmsTokenHandler(configuration);
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
            PhoneNumber = model.ContactNumber,
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
    /// Authentication
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password)) return Unauthorized();

        var userRoles = await _userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
        {
            new("jti", Guid.NewGuid().ToString()),
            new("username", user.UserName),
            new("sub", user.Id)
        };

        authClaims.AddRange(userRoles.Select(userRole => new Claim("roles", userRole)));

        return Ok(new ApiResponse<object>
        {
            Data = new
            {
                access_token = _tokenHandler.CreateToken(authClaims),
                refresh_token = _tokenHandler.CreateRefreshToken(user.Id)
            }
        });
    }


    /// <summary>
    /// Refresh Token 
    /// </summary>
    /// <param name="refreshToken">Refresh token</param>
    /// <returns></returns>
    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshToken(
        [FromHeader(Name = "refresh_token"), BindRequired]
        string refreshToken)
    {
        if (!ModelState.IsValid)
        {
            return Unauthorized();
        }

        if (!_tokenHandler.IsValidateToken(refreshToken)) return Unauthorized();

        var token = new JwtSecurityToken(refreshToken);

        var user = await _userManager.FindByIdAsync(token.Subject);

        var userRoles = await _userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
        {
            new("jti", Guid.NewGuid().ToString()),
            new("username", user.UserName),
            new("sub", user.Id)
        };

        authClaims.AddRange(userRoles.Select(userRole => new Claim("roles", userRole)));

        return Ok(new ApiResponse<object>
        {
            Data = new
            {
                access_token = _tokenHandler.CreateToken(authClaims),
                refresh_token = _tokenHandler.CreateRefreshToken(user.Id)
            }
        });
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("forgot-password")]
    public async Task<IActionResult> ForgotPassword([Required] string email)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid request");
        }

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return BadRequest("Invalid request");
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var callback = Url.Action("ResetPassword", "Authenticate", new { token, email = user.Email }, Request.Scheme);
     
        var message = new Message(new string[] { user.Email }, "Reset password token", callback);
        await _emailSender.SendEmailAsync(message);
        
        return Ok("Password reset link sent successfully.");
    }
    
    
    [HttpPost]
    [AllowAnonymous]
    [Route("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid request");
        }

        var user = await _userManager.FindByIdAsync(model.Email);
        if (user == null)
        {
            // User not found
            return BadRequest("Invalid request");
        }

        var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
        if (result.Succeeded)
        {
            // Password successfully reset
            return Ok("Password reset successful.");
        }
        else
        {
            // Password reset failed
            return BadRequest("Password reset failed. Please try again.");
        }
    }

   
}