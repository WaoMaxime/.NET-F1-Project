using System.Security.Claims;
using BusinessLayer;
using DataAccessLayer.EF;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Controllers;
using UI.DTO;

namespace UI.Controllers;

[Authorize]
public class F1CarController : Controller
{  
    
    private readonly IManager _manager;
    private readonly UserManager<IdentityUser> _userManager;
    
    
    public F1CarController(IManager manager, UserManager<IdentityUser> userManager)
    {
        _manager = manager;
        _userManager = userManager;
    }

    [AllowAnonymous]
    public IActionResult Index()    
    {
        var cars = _manager.GetAllF1CarsWithDetails();
        return View(cars);
    }

    [AllowAnonymous]
    public IActionResult Details(int id)
    {
        var car = _manager.GetF1CarWithDetails(id);
        if (car == null)
        {
            return NotFound();
        }
        var user = _userManager.FindByIdAsync(car.UserId!);
        var username = user.Result?.UserName;
        ViewData["MaintainerUsername"] = username;
        return View(car);
    }
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public ViewResult Add()
    {
        return View();
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult Add(F1Car newCar)
    {
        if (!ModelState.IsValid)
        {
            return View(newCar);
        }
        _manager.AddF1Car(
            newCar.Team,
            newCar.Chasis,
            newCar.ConstructorsPosition,
            newCar.DriversPositions,
            newCar.ManufactureDate,
            newCar.Tyres,
            newCar.User,
            newCar.EnginePower);
        return RedirectToAction("Details", new { Id = newCar.Id });
    }
}
