using Microsoft.AspNetCore.Mvc;
using Mono.Contracts.Repositories;
using AutoMapper;
using Mono.Data;
using Mono.Contracts.Models;
using Mono.Contracts.Services;

namespace Mono.Mvc.Controllers;

public class VehicleController : Controller
{
    private readonly ILogger<VehicleController> _logger;
    private IVehicleService _vehicleService;

    public VehicleController(ILogger<VehicleController> logger)
    {
        _logger = logger;
        _vehicleService = NinjectProvider.Get<IVehicleService>();
    }

    public async Task<IActionResult> Index(){

        return View();
    }

    public async Task<IActionResult> Manufacturers(){
        

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateManufacturer(string name){

        return RedirectToAction(nameof(Manufacturers));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteManufacturer(int id){

        return RedirectToAction(nameof(Manufacturers));
    }

    public async Task<IActionResult> Manufacturer(int id){

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateManufacturerName(int id, string name){

        return RedirectToAction(nameof(Manufacturer), new {id = id});
    }

    [HttpPost]
    public async Task<IActionResult> AddModelToManufacturer(int id, int modelId){
        
        return RedirectToAction(nameof(Manufacturer), new {id = id});
    }

    [HttpPost]
    public async Task<IActionResult> RemoveModelFromManufacturer(int id, int modelId){

        return RedirectToAction(nameof(Manufacturer), new {id = id});
    }

    public async Task<IActionResult> VehicleModel(int id){

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateVehicleModel(CreateVehicleModelVM viewModel){

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> UpdateVehicleModel(UpdateVehicleModelVM viewModel){

        return RedirectToAction(nameof(Index));
    }


}