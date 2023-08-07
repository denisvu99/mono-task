using Microsoft.AspNetCore.Mvc;
using Mono.Contracts.Repositories;
using AutoMapper;
using Mono.Data;
using Mono.Contracts.Models;
using Mono.Contracts.Services;
using X.PagedList;

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

    public async Task<IActionResult> Index(string sortOrder, int? filter, int? page){
        ViewBag.ManSort = String.IsNullOrEmpty(sortOrder) ? "man_desc" : "";
        ViewBag.ModelSort = sortOrder == "model_asc" ? "model_desc" : "model_asc";
        ViewBag.CurrentSort = sortOrder;
        ViewBag.CurrentFilter = filter;

        var models = await _vehicleService.ListVehicleModels(sortOrder, filter);

        ViewBag.Manufacturers = await _vehicleService.ListViewBagManufacturers();

        var pagedModel = await models.ToPagedListAsync(page ?? 1,10);
        return View(pagedModel);
    }

    public async Task<IActionResult> Manufacturers(string sortOrder, int? filter, int? page){
        ViewBag.ManSort = String.IsNullOrEmpty(sortOrder) ? "man_desc" : "";
        ViewBag.CountSort = sortOrder == "count_asc" ? "count_desc" : "count_asc";
        ViewBag.CurrentSort = sortOrder;

        IEnumerable<ManufacturersVM> manufacturers = await _vehicleService.ListManufacturers(sortOrder, filter);

        var pagedManufacturers = await manufacturers.ToPagedListAsync(page ?? 1,10);
        return View(pagedManufacturers);
    }

    [HttpPost]
    public async Task<IActionResult> CreateManufacturer(string name){
        var isCreated = await _vehicleService.CreateManufacturer(name);
        if (isCreated) Response.StatusCode = 201;
        else Response.StatusCode = 400;

        return RedirectToAction(nameof(Manufacturers));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteManufacturer(int id){
        var isDeleted = await _vehicleService.DeleteManufacturer(id);
        if (isDeleted == null) Response.StatusCode = 404;
        else Response.StatusCode = 200;

        return RedirectToAction(nameof(Manufacturers));
    }

    public async Task<IActionResult> Manufacturer(int id){
        var manufacturer = await _vehicleService.GetManufacturer(id);
        if (manufacturer == null) Response.StatusCode = 404;
        else Response.StatusCode = 200;

        return View(manufacturer);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateManufacturerName(int id, string name){
        var isUpdate = await _vehicleService.UpdateManufacturerName(id, name);
        if (isUpdate == null) Response.StatusCode = 404;
        else Response.StatusCode = 200;

        return RedirectToAction(nameof(Manufacturer), new { id });
    }

    [HttpPost]
    public async Task<IActionResult> AddModelToManufacturer(int id, int modelId){
        var isAdded = await _vehicleService.AddModelToManufacturer(id, modelId);
        if (isAdded == null) Response.StatusCode = 404;
        else Response.StatusCode = 200;

        return RedirectToAction(nameof(Manufacturer), new { id });
    }

    [HttpPost]
    public async Task<IActionResult> RemoveModelFromManufacturer(int id, int modelId){
        var isRemoved = await _vehicleService.RemoveModelFromManufacturer(id, modelId);
        if (isRemoved == null) Response.StatusCode = 404;
        else Response.StatusCode = 200;

        return RedirectToAction(nameof(Manufacturer), new { id });
    }

    public async Task<IActionResult> VehicleModel(int id){
        var model = await _vehicleService.GetVehicleModel(id);
        if (model == null) Response.StatusCode = 404;
        else Response.StatusCode = 200;
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> CreateVehicleModel(CreateVehicleModelVM viewModel){
        var isCreated = await _vehicleService.CreateVehicleModel(viewModel);
        if (isCreated) Response.StatusCode = 201;
        else Response.StatusCode = 400;

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> UpdateVehicleModel(UpdateVehicleModelVM viewModel){
        var isUpdated = await _vehicleService.UpdateVehicleModel(viewModel);
        if (isUpdated == null) Response.StatusCode = 404;
        else Response.StatusCode = 200;

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteVehicleModel(int id){
        var isDeleted = await _vehicleService.DeleteVehicleModel(id);
        if (isDeleted == null) Response.StatusCode = 404;
        else Response.StatusCode = 200;

        return RedirectToAction(nameof(Index));
    }

}