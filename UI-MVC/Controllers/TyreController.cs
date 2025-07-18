﻿using BusinessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class TyreController : Controller
{
    private readonly IManager _manager;

    public TyreController(IManager manager)
    {
        _manager = manager;
    }

    
    public IActionResult Details(int id)
    {
        var tyre = _manager.GetTyreById(id);
        if (tyre == null)
        {
            return NotFound("Tyre not found.");
        }
        return View(tyre);
    }
}
