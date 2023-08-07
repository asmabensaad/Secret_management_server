using System.Diagnostics;
using System.Security.Authentication;
using System.Security.Claims;
using DataAccess.Database;
using DataAccess.Models.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Services.Auth.Controllers;

//TODO: Follow naming convention
//TODO: Remove commented code
//TODO: Fix spelling
[Route("api/[controller]")]
[ApiController]
public class UsersController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    private readonly UserManager<ApplicationUser> _userManager;

    public UsersController(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager
    )
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    /// <summary>
    /// Get List Of All  Users
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("GetAllUsers")]
    public async Task<IActionResult> GetUsers()
    {
        var allUsers = await _userManager.Users.ToListAsync();
        var userInformationList = allUsers.Select(u => new RegisterModel
        {
            Id = u.Id,
            Username = u.UserName,
            Email = u.Email,
            ContactNumber = u.PhoneNumber
        }).ToList();
        return Ok(userInformationList);
    }

    /// <summary>
    /// Get Current User
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getCurrentUser")]
    public async Task<IActionResult> GetUser()
    {
        if (User.Identity is not {IsAuthenticated: true}) return BadRequest("user not authenticated");
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            return Ok(new
            {
                userId = user.Id,
                user.UserName,
                user.Email
            });
        }

        return BadRequest("user not authenticated");
    }

    [Authorize]
    [HttpGet]
    public int GetLoggedUserId()
    {
        if (User.Identity is {IsAuthenticated: false})
            throw new AuthenticationException();

        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        Debug.Assert(userId != null, nameof(userId) + " != null");
        return int.Parse(userId);
    }

    //
    // [Route("UpdateUser")]
    // [HttpPost]
    // public async Task<IActionResult> UpdateUser(RegisterModel model )
    // {
    //     ApplicationUser user = await _userManager.FindByEmailAsync(model.Email);
    //     if (user != null)
    //     {
    //         if (!string.IsNullOrEmpty(model.Email))
    //             user.Email = model.Email;
    //         else 
    //             ModelState.AddModelError("","email cannot be empty");
    //         if (!string.IsNullOrEmpty(model.Username))
    //             user.UserName = model.Username;
    //    
    //         
    //         
    //
    //     }
    // var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
    // var user = await _userManager.FindByIdAsync(userId);
    // if (user == null)
    // {
    //     return BadRequest("user not found");
    // }
    //
    // user.Email = model.Email;
    // user.UserName = model.Username;
    // user.PhoneNumber = model.ContactNumber;
    //
    // var result = await _userManager.UpdateAsync(user);
    // if (result.Succeeded)
    // {
    //     return Ok("user updated successfuly");
    // }
    // else
    // {
    //     var errors = result.Errors.Select(e => e.Description);
    //     return BadRequest(new { errors });
    // }
}