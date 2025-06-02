using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.Services;
using Domain.DTOs;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    // [Authorize]
    [HttpGet("profile/{id:guid}")]
    public async Task<IActionResult> GetProfile(Guid id)
    {
        var result = await _userService.ProfileInfoUser(id);
        
        if(result == null)
            return NotFound(new { message = "User not found" });
        
        return Ok(result);
    }
}