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
    public ActionResult<Player> RecordGoal(string playerName)
    {
        var updatedPlayer = _service.RecordGoal(playerName);
        return Ok(updatedPlayer);
    }

    [HttpPost("record-goals")]
    public ActionResult<IEnumerable<Player>> RecordGoals([FromBody] IEnumerable<string> playerNames)
    {
        var updatedPlayers = _service.RecordGoals(playerNames);
        return Ok(updatedPlayers);
    }
}