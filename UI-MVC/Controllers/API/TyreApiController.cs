using System.Collections;
using BusinessLayer;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using UI.DTO;

[Route("api/[controller]")]
[ApiController]
public class TyreApiController : Controller
{
    private readonly IManager _manager;
    private readonly UserManager<IdentityUser> _userManager;

    public TyreApiController(IManager manager, UserManager<IdentityUser> userManager)
    {
        _manager = manager;
        _userManager = userManager;
    }

    [HttpGet("GetTyresForCar/{carId}")]
    public IActionResult GetTyresForCar(int carId)
    {
        var tyres = _manager.GetCarTyresForCarById(carId);
        var tyreDtos = tyres.Select(tyre => new CarTyreDto
        {
            CarId = tyre.CarId,
            Tyre = tyre.Tyre,
            TyrePressure = tyre.TyrePressure,
            OperationalTemperature = tyre.OperationalTemperature,
        }).ToList();

        if (!tyreDtos.Any())
        {
            return NoContent(); 
        }

        return Ok(tyreDtos); 
    }

    [HttpGet("GetTyreTypes")]
    public ActionResult<IEnumerable<string>> GetTyreTypes()
    {
        var tyreTypes = Enum.GetValues(typeof(TyreType))
            .Cast<TyreType>()
            .Select(t => t.ToString())
            .ToArray();

        return Ok(tyreTypes);
    }

    [HttpPost("AddTyreToCar")]
    public IActionResult AddTyreToCar(/*[FromBody]*/ CarTyreDto newCarTyreDto)
    {
        try
        {
            if (User.Identity != null && !User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            var createdCarTyre = _manager.AddTyreToCar(
                newCarTyreDto.CarId,
                newCarTyreDto.Tyre,
                newCarTyreDto.TyrePressure,
                newCarTyreDto.OperationalTemperature
            );
            return CreatedAtAction(nameof(GetTyresForCar),
                new { carId = createdCarTyre.CarId },
                new CarTyreDto
                {
                    CarId = createdCarTyre.CarId,
                    Tyre = createdCarTyre.Tyre,
                    TyrePressure = createdCarTyre.TyrePressure,
                    OperationalTemperature = createdCarTyre.OperationalTemperature
                });
            
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
        }
    }
}
