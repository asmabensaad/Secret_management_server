using System.Security.Claims;
using DataAccess.Database;
using DataAccess.Models.AuthService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace Services.Auth.Controllers;

//TODO: Follow naming convention
//TODO: Remove commented code
//TODO: Fix spelling
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
            ContactNumber = u.PhoneNumber,
           
        }).ToList();
        return Ok(userInformationList);
    }

    /// <summary>
    /// Get the Current User
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("getCurrentUser")]
    public async Task<IActionResult> GetUser()
    {
        if (User.Identity is not { IsAuthenticated: true }) return BadRequest("user not authenticated");
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            return Ok(new
            {
                userId = user.Id,
                user.UserName,
                user.Email,
                user.PhoneNumber
                
                
            });
        }

        return BadRequest("user not authenticated");
    }

    /// <summary>
    /// update user
    /// </summary>
    /// <param name="id"></param>
    /// <param name="username"></param>
    /// <param name="email"></param>
    /// <param name="contactNumber"></param>
    /// <returns></returns>
    [Route("UpdateUser/{id}")]
    [HttpPost]
    public async Task<IActionResult> UpdateUser(string id,string? username,string? email,string? contactNumber)
    {


       
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound("User not Found");
        }
        

        user.UserName = username;
        user.Email = email;
        user.PhoneNumber = contactNumber;
        var updatedResult = await _userManager.UpdateAsync(user);
        if (updatedResult.Succeeded)
        {
            return Ok("User updated successfully");
        }

        var errors = updatedResult.Errors.Select(error => error.Description);
        return BadRequest(errors);

    }
/// <summary>
/// Delete User 
/// </summary>
/// <param name="userId"></param>
/// <returns></returns>

    [Route("DeleteUser")]
    [HttpDelete]
    public async Task<IActionResult> DeleteUser([FromQuery (Name = "userId"), BindRequired]string? userId)
    {
        if (userId == null )
        {
            return BadRequest("Invalid data format");
        }
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound("user not found");
        }

        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            await _signInManager.SignOutAsync();
            return Ok("user deleted successfully");
        }

        var errors = result.Errors.Select(error => error.Description);
        return BadRequest(errors);
    }
}