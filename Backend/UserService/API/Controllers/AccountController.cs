using Application.Interfaces;
using Application.Services;
using Domain.Models.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly JwtAuthenticationService _jwtAuthenticationService;
    
    public AccountController(JwtAuthenticationService jwtAuthenticationService) =>
        _jwtAuthenticationService = jwtAuthenticationService;

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseModel>> Login([FromBody] LoginRequestModel request)
    {
        var result = await _jwtAuthenticationService.Authenticate(request);
        return result is not null ? result : Unauthorized();
    }
    
}