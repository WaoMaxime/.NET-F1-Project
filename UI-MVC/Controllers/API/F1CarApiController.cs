using DataAccessLayer.EF;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UI.DTO;

namespace UI_MVC.Controllers.API;

[ApiController]
[Route("api/F1Cars")]
public class F1CarApiController : ControllerBase
{
    private readonly F1CarDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public F1CarApiController(F1CarDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateF1CarHp(int id, [FromBody] UpdateF1CarHpDto car)
    {
        var f1Car = await _context.F1Cars.FindAsync(id);
        if (f1Car == null)
            return NotFound("Car not found.");

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        if (f1Car.UserId != user.Id)
            return Forbid();

        f1Car.EnginePower = car.F1CarHp;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Horsepower updated successfully", updatedHp = car.F1CarHp });
    }
}
