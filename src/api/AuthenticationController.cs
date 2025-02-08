using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IJwtTokenService _tokenService;

    public AuthenticationController(IJwtTokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpGet]
    public IActionResult GetToken()
    {
        var token = _tokenService.GenerateToken();
        return Ok(new { token });
    }
}