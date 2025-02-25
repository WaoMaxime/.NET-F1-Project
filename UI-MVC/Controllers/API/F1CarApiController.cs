using BusinessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UI.DTO;

namespace UI_MVC.Controllers.API;

[ApiController]
[Route("api/F1Cars")]
public class F1CarApiController : ControllerBase
{
    private readonly Manager _manager;
    private readonly UserManager<IdentityUser> _userManager;

    public F1CarApiController(UserManager<IdentityUser> userManager, Manager manager)
    {
        _userManager = userManager;
        _manager = manager;
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateF1CarHp(int id, /*[FromBody]*/ UpdateF1CarHpDto car)
    {
        var f1Car = _manager.ChangeHpF1Car(id, car.F1CarHp);
        if (f1Car == null)
            return NotFound("Car not found.");

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        if (user.Id != f1Car.UserId && !User.IsInRole("Admin"))
            return Forbid();

        return Ok(new { message = "Horsepower updated successfully", updatedHp = car.F1CarHp });
    }
}
