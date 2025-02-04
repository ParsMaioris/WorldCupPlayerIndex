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
    public ActionResult<IEnumerable<Player>> GetPlayersOlderThan(int age)
    {
        var players = _service.GetPlayersOlderThan(age);
        return Ok(players);
    }

    [HttpGet("nationality/{nationality}")]
    public ActionResult<IEnumerable<Player>> GetPlayersByNationality(string nationality)
    {
        var players = _service.GetPlayersByNationality(nationality);
        return Ok(players);
    }

    [HttpGet("names")]
    public ActionResult<IEnumerable<string>> GetPlayerNames()
    {
        var names = _service.GetPlayerNames();
        return Ok(names);
    }

    [HttpGet("total-goals")]
    public ActionResult<int> GetTotalGoals()
    {
        var total = _service.GetTotalGoals();
        return Ok(total);
    }

    [HttpGet("grouped-by-nationality")]
    public ActionResult<IEnumerable<IGrouping<string, Player>>> GetPlayersGroupedByNationality()
    {
        var grouped = _service.GetPlayersGroupedByNationality();
        return Ok(grouped);
    }

    [HttpGet("ordered-by-performance")]
    public ActionResult<IEnumerable<Player>> GetPlayersOrderedByPerformance([FromQuery] bool descending = true)
    {
        var players = _service.GetPlayersOrderedByPerformanceScore(descending);
        return Ok(players);
    }

    [HttpGet("any-player-reached")]
    public ActionResult<bool> AnyPlayerReachedGoalThreshold([FromQuery] int threshold)
    {
        var reached = _service.AnyPlayerReachedGoalThreshold(threshold);
        return Ok(reached);
    }

    [HttpGet("top-performer")]
    public ActionResult<Player> GetTopPerformer()
    {
        try
        {
            var player = _service.GetTopPerformer();
            return Ok(player);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("paged")]
    public ActionResult<IEnumerable<Player>> GetPlayersPaged([FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        var players = _service.GetPlayersPaged(pageNumber, pageSize);
        return Ok(players);
    }

    [HttpGet("distinct-nationalities")]
    public ActionResult<IEnumerable<string>> GetDistinctNationalities()
    {
        var nationalities = _service.GetDistinctNationalities();
        return Ok(nationalities);
    }
}