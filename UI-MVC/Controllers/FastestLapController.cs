using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
[Authorize]
public class FastestLapController : Controller
{
    public ViewResult Index()
    {
        return View();
    }
}