using Application.Interfaces;
using Application.Services;
using Domain.Models.JwtTokenApi;
using Domain.Models.LoginApi;
using Domain.Models.RefreshTokenApi;
using Domain.Models.RegisterApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly JwtAuthenticationService _jwtAuthenticationService;
    private readonly IAuthService _authService;

    public AccountController(JwtAuthenticationService jwtAuthenticationService, IAuthService authService)
    {
        _jwtAuthenticationService = jwtAuthenticationService;
        _authService = authService;
    }
       

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginRequestModel request)
    {
        var result = await _authService.LoginAsync(request);
        if(result == null)
            return Unauthorized(new { message = "Invalid email or password" });
        return Ok(new
        {
            accesstoken = result.AccessToken,
            refreshToken = result.RefreshToken,
        });
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<ActionResult<LoginResponseModel?>> Refresh([FromBody] RefreshRequestModel request)
    {
        if(string.IsNullOrWhiteSpace(request.Token))
            return BadRequest("Invalid Token");
        
        var result = await _jwtAuthenticationService.ValidateRefreshToken(request.Token);
        return result is not null ? result : Unauthorized();
    }
    
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestApi register)
    {
        var result = await _authService.CreateUserAsync(register);
        if (result)
            return Ok(new { message = "User created successfully" });
        else
            return BadRequest(new { message = "Invalid user data" });
        
    }

    
}