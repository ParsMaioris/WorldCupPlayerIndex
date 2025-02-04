using Microsoft.AspNetCore.Mvc;

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
    public ActionResult<IEnumerable<Player>> GetVeteranPlayers()
    {
        var veterans = _service.GetVeteranPlayers();
        return Ok(veterans);
    }
}