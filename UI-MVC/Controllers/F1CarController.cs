using BusinessLayer;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers;

public class F1CarController : Controller
{
    private readonly IManager _manager;

    public F1CarController(IManager manager)
    {
        _manager = manager;
    }

    public IActionResult Index()
    {
        var cars = _manager.GetAllF1CarsWithDetails();
        return View(cars);
    }

    public IActionResult Details(int id)
    {
        var car = _manager.GetF1CarWithDetails(id);
        if (car == null)
        {
            return NotFound();
        }
        return View(car);
    }
    
    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }

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
            newCar.EnginePower);
        
        return RedirectToAction("Index");
    }
}
