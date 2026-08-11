using FurnitureERP.Application.Common.Interfaces;
using FurnitureERP.Application.Security.DTOs;
using FurnitureERP.Application.Security.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FurnitureERP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    private readonly ICurrentUserService
        _currentUserService;

    public AuthController(
        IAuthService authService,
        ICurrentUserService currentUserService)
    {
        _authService = authService;

        _currentUserService =
            currentUserService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestdDTO request)
    {
        var user = await _authService.Register(request);

        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto request)
    {
        var result = await _authService.Login(request);

        return Ok(result);
    }



    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        return Ok(new
        {
            UserId = _currentUserService.UserId,
            UserName = _currentUserService.UserName,
            Permissions =
                _currentUserService.Permissions
        });
    }

}