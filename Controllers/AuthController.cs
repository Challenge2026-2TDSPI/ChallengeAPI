
using ChallengeAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ChallengeAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly TokenService _tokenService;

    public AuthController(TokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] UserLoginRequest model)
    {
        if (model.Username == "admin" &&
            model.Password == "Eduardo@28422915")
        {
            var token = _tokenService.GenerateToken(model.Username);
            return Ok(new { token });
        }

        return Unauthorized();
    }

    [HttpGet("dados-protegidos")]
    [Authorize]
    public IActionResult GetDadosProtegidos()
    {
        var username = User.FindFirst(
            ClaimTypes.NameIdentifier)?.Value;

        return Ok(new
        {
            message = $"Olá {username}! Você está autenticado."
        });
    }

    public class UserLoginRequest
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
