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
    private readonly IAuthService _authService;
    private readonly IUserService _userService;
    private readonly JwtService _jwtService;

    public UserController(IAuthService authService, IUserService userService, JwtService jwtService)
    {
        _userService = userService;
        _authService = authService;
        _jwtService = jwtService;
    }
    
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO register)
    {
        var result = await _authService.CreateUserAsync(register);
        if (result)
            return Ok(new { message = "User created successfully" });
        else
            return BadRequest(new { message = "Invalid user data" });
        
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO login)
    {
        var Response = await _jwtService.CreateAccessTokenFromUser(login.Email, login.Password);
        if(Response == null)
            return BadRequest(new { message = "Invalid email or password" });
        
        return Ok(Response);
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
    
    
    // [Authorize(Roles = "Admin")]
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _userService.GetAlLUser();
        return Ok(result);
    }
}