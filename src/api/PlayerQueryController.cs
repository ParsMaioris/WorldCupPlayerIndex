using Microsoft.AspNetCore.Mvc;

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
        var players = await _service.GetPlayersOlderThanAsync(age);
        return Ok(players);
    }

    [HttpGet("nationality/{nationality}")]
    public async Task<ActionResult<IEnumerable<Player>>> GetPlayersByNationalityAsync(string nationality)
    {
        var players = await _service.GetPlayersByNationalityAsync(nationality);
        return Ok(players);
    }

    [HttpGet("names")]
    public async Task<ActionResult<IEnumerable<string>>> GetPlayerNamesAsync()
    {
        var names = await _service.GetPlayerNamesAsync();
        return Ok(names);
    }

    [HttpGet("total-goals")]
    public async Task<ActionResult<int>> GetTotalGoalsAsync()
    {
        var total = await _service.GetTotalGoalsAsync();
        return Ok(total);
    }

    [HttpGet("grouped-by-nationality")]
    public async Task<ActionResult<IDictionary<string, List<Player>>>> GetPlayersGroupedByNationalityAsync()
    {
        var grouped = await _service.GetPlayersGroupedByNationalityAsync();
        return Ok(grouped);
    }

    [HttpGet("ordered-by-performance")]
    public async Task<ActionResult<IEnumerable<Player>>> GetPlayersOrderedByPerformanceAsync([FromQuery] bool descending = true)
    {
        var players = await _service.GetPlayersOrderedByPerformanceScoreAsync(descending);
        return Ok(players);
    }

    [HttpGet("any-player-reached")]
    public async Task<ActionResult<bool>> AnyPlayerReachedGoalThresholdAsync([FromQuery] int threshold)
    {
        var reached = await _service.AnyPlayerReachedGoalThresholdAsync(threshold);
        return Ok(reached);
    }

    [HttpGet("top-performer")]
    public async Task<ActionResult<Player>> GetTopPerformerAsync()
    {
        var player = await _service.GetTopPerformerAsync();
        return Ok(player);
    }

    [HttpGet("paged")]
    public async Task<ActionResult<IEnumerable<Player>>> GetPlayersPagedAsync([FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        var players = await _service.GetPlayersPagedAsync(pageNumber, pageSize);
        return Ok(players);
    }

    [HttpGet("distinct-nationalities")]
    public async Task<ActionResult<IEnumerable<string>>> GetDistinctNationalitiesAsync()
    {
        var nationalities = await _service.GetDistinctNationalitiesAsync();
        return Ok(nationalities);
    }
}