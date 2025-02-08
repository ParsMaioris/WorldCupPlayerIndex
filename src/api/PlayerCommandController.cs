using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PlayerCommandController : ControllerBase
{
    private readonly IPlayerCommandService _service;

    public PlayerCommandController(IPlayerCommandService service)
    {
        _service = service;
    }

    [HttpPost("{playerName}/record-goal")]
    public async Task<ActionResult<Player>> RecordGoalAsync(string playerName)
    {
        try
        {
            var updatedPlayer = await _service.RecordGoalAsync(playerName);
            return Ok(updatedPlayer);
        }
        catch (DomainException ex)
        {
            return StatusCode(ex.StatusCode, new { error = ex.Message });
        }
    }

    [HttpPost("record-goals")]
    public async Task<ActionResult<IEnumerable<Player>>> RecordGoalsAsync([FromBody] IEnumerable<string> playerNames)
    {
        try
        {
            var updatedPlayers = await _service.RecordGoalsAsync(playerNames);
            return Ok(updatedPlayers);
        }
        catch (DomainException ex)
        {
            return StatusCode(ex.StatusCode, new { error = ex.Message });
        }
    }
}