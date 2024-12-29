using Microsoft.AspNetCore.Mvc;
using BusinessLayer;
using Domain;

[Route("api/[controller]")]
[ApiController]
public class FastestLapApiController : ControllerBase
{
    private readonly IManager _manager;

    public FastestLapApiController(IManager manager)
    {
        _manager = manager;
    }
    
    [HttpGet]
    public IActionResult GetAll()
    {
        var fastestLaps = _manager.GetAllFastestLaps();
        return Ok(fastestLaps);
    }

    [HttpPost]
    public IActionResult Create([FromBody] FastestLap newLap)
    {
        _manager.AddFastestLap(
            newLap.Circuit,
            newLap.AirTemperature,
            newLap.TrackTemperature,
            newLap.LapTime,
            newLap.DateOfRecord,
            newLap.Car,
            newLap.Race
        );
        return CreatedAtAction(nameof(GetAll), new { id = newLap.Id }, newLap);
    }
}
