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
        var models = await _vehicleService.ListVehicleModels();

        return View(models);
    }

    public async Task<IActionResult> Manufacturers(){
        IEnumerable<ManufacturersVM> manufacturers = await _vehicleService.ListManufacturers();

        return View(manufacturers);
    }

    [HttpPost]
    public async Task<IActionResult> CreateManufacturer(string name){
        var manufacturers = await _vehicleService.CreateManufacturer(name);

        return View(nameof(Manufacturers), manufacturers);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteManufacturer(int id){
        var isDeleted = await _vehicleService.DeleteManufacturer(id);

        return RedirectToAction(nameof(Manufacturers));
    }

    public async Task<IActionResult> Manufacturer(int id){
        var manufacturer = await _vehicleService.GetManufacturer(id);

        return View(manufacturer);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateManufacturerName(int id, string name){
        var model = await _vehicleService.UpdateManufacturerName(id, name);

        return RedirectToAction(nameof(Manufacturer), new {id = id});
    }

    [HttpPost]
    public async Task<IActionResult> AddModelToManufacturer(int id, int modelId){
        var isAdded = await _vehicleService.AddModelToManufacturer(id, modelId);

        return RedirectToAction(nameof(Manufacturer), new {id = id});
    }

    [HttpPost]
    public async Task<IActionResult> RemoveModelFromManufacturer(int id, int modelId){
        var isRemoved = await _vehicleService.RemoveModelFromManufacturer(id, modelId);

        return RedirectToAction(nameof(Manufacturer), new {id = id});
    }

    public async Task<IActionResult> VehicleModel(int id){
        var model = await _vehicleService.GetVehicleModel(id);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> CreateVehicleModel(CreateVehicleModelVM viewModel){
        var model = await _vehicleService.CreateVehicleModel(viewModel);

        return View(nameof(Index), model);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateVehicleModel(UpdateVehicleModelVM viewModel){
        var isUpdated = await _vehicleService.UpdateVehicleModel(viewModel);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteVehicleModel(int id){
        var isDeleted = await _vehicleService.DeleteVehicleModel(id);
        return RedirectToAction(nameof(Index));
    }

}