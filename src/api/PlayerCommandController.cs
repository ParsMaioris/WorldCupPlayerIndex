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
        try
        {
            var updatedPlayer = _service.RecordGoal(playerName);
            return Ok(updatedPlayer);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("record-goals")]
    public ActionResult<IEnumerable<Player>> RecordGoals([FromBody] IEnumerable<string> playerNames)
    {
        try
        {
            var updatedPlayers = _service.RecordGoals(playerNames);
            return Ok(updatedPlayers);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}