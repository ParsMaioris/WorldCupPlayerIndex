using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PlayerQueryController : ControllerBase
{
    private readonly IPlayerQueryService _service;

    public PlayerQueryController(IPlayerQueryService service)
    {
        _service = service;
    }

    [HttpGet("older-than/{age}")]
    public async Task<ActionResult<IEnumerable<Player>>> GetPlayersOlderThanAsync(int age)
    {
        try
        {
            var players = await _service.GetPlayersOlderThanAsync(age);
            return Ok(players);
        }
        catch (QueryInvalidException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (QueryNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [HttpGet("nationality/{nationality}")]
    public async Task<ActionResult<IEnumerable<Player>>> GetPlayersByNationalityAsync(string nationality)
    {
        try
        {
            var players = await _service.GetPlayersByNationalityAsync(nationality);
            return Ok(players);
        }
        catch (QueryInvalidException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (QueryNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [HttpGet("names")]
    public async Task<ActionResult<IEnumerable<string>>> GetPlayerNamesAsync()
    {
        try
        {
            var names = await _service.GetPlayerNamesAsync();
            return Ok(names);
        }
        catch (QueryNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [HttpGet("total-goals")]
    public async Task<ActionResult<int>> GetTotalGoalsAsync()
    {
        try
        {
            var total = await _service.GetTotalGoalsAsync();
            return Ok(total);
        }
        catch (QueryNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [HttpGet("grouped-by-nationality")]
    public async Task<ActionResult<IDictionary<string, List<Player>>>> GetPlayersGroupedByNationalityAsync()
    {
        try
        {
            var grouped = await _service.GetPlayersGroupedByNationalityAsync();
            return Ok(grouped);
        }
        catch (QueryNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [HttpGet("any-player-reached")]
    public async Task<ActionResult<bool>> AnyPlayerReachedGoalThresholdAsync([FromQuery] int threshold)
    {
        try
        {
            var reached = await _service.AnyPlayerReachedGoalThresholdAsync(threshold);
            return Ok(reached);
        }
        catch (QueryInvalidException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (QueryNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [HttpGet("top-performer")]
    public async Task<ActionResult<Player>> GetTopPerformerAsync()
    {
        try
        {
            var player = await _service.GetTopPerformerAsync();
            return Ok(player);
        }
        catch (QueryNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [HttpGet("paged")]
    public async Task<ActionResult<IEnumerable<Player>>> GetPlayersPagedAsync([FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        try
        {
            var players = await _service.GetPlayersPagedAsync(pageNumber, pageSize);
            return Ok(players);
        }
        catch (QueryInvalidException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (QueryNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [HttpGet("distinct-nationalities")]
    public async Task<ActionResult<IEnumerable<string>>> GetDistinctNationalitiesAsync()
    {
        try
        {
            var nationalities = await _service.GetDistinctNationalitiesAsync();
            return Ok(nationalities);
        }
        catch (QueryNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }
}