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
    public Task<IActionResult> UpdateF1CarHp(int id, /*[FromBody]*/ UpdateF1CarHpDto car)
    {
        var presentCar = _manager.GetF1Car(id);
        if (presentCar == null)
        {
            return Task.FromResult<IActionResult>(NotFound());
        }
        var f1Car = _manager.ChangeHpF1Car(id, car.F1CarHp);
        var user = _userManager.GetUserId(User);
        if (user == null)
            return Task.FromResult<IActionResult>(Unauthorized());

        if (user != f1Car.UserId && !User.IsInRole("Admin"))
            return Task.FromResult<IActionResult>(Forbid());

        return Task.FromResult<IActionResult>(Ok(new { message = "Horsepower updated successfully", updatedHp = car.F1CarHp }));
    }
}
