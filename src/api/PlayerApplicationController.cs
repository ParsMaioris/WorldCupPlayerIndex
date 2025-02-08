using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PlayerApplicationController : ControllerBase
{
    private readonly IPlayerApplicationService _service;
    public PlayerApplicationController(IPlayerApplicationService service)
    {
        _service = service;
    }

    [HttpGet("veterans")]
    public async Task<ActionResult<IEnumerable<Player>>> GetVeteranPlayersAsync()
    {
        var veterans = await _service.GetVeteranPlayersAsync();
        return Ok(veterans);
    }
}