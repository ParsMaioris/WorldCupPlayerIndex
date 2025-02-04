using Microsoft.AspNetCore.Mvc;

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
        var updatedPlayer = await _service.RecordGoalAsync(playerName);
        return Ok(updatedPlayer);
    }

    [HttpPost("record-goals")]
    public async Task<ActionResult<IEnumerable<Player>>> RecordGoalsAsync([FromBody] IEnumerable<string> playerNames)
    {
        var updatedPlayers = await _service.RecordGoalsAsync(playerNames);
        return Ok(updatedPlayers);
    }
}