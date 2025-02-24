using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class FastestLapController : Controller
{
    [Authorize]
    public IActionResult Index()
    {
        return View();
    }
}