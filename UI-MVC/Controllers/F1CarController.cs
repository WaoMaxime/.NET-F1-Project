using BusinessLayer;
using Microsoft.AspNetCore.Mvc;

namespace UI;

public class F1CarController : Controller
{
    
    private readonly IManager _manager;

    public F1CarController(IManager manager)
    {
        _manager = manager;
    }
    
    public IActionResult Index()
    {
        var cars = _manager.GetAllF1Cars(); 
        return View(cars);
    }

    
    public IActionResult Add()
    {
        return View();
    }
    
    public IActionResult Details(int id)
    {
        var car = _manager.GetF1Car(id);
        if (car == null)
        {
            return NotFound();
        }
        return View(car);
    }
}