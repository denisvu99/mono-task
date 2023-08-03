using Microsoft.AspNetCore.Mvc;
using Mono.Contracts.Repositories;
using AutoMapper;
using Mono.Data;
using Mono.Contracts.Models;

namespace Mono.Mvc.Controllers;

public class VehicleController : Controller
{
    private readonly ILogger<VehicleController> _logger;
    private IVehicleMakeRepository _vmakeRepo;
    private IVehicleModelRepository _vmodelRepo;
    private IMapper _mapper;

    public VehicleController(ILogger<VehicleController> logger)
    {
        _logger = logger;
        _vmakeRepo = NinjectProvider.Get<IVehicleMakeRepository>();
        _vmodelRepo = NinjectProvider.Get<IVehicleModelRepository>();
        _mapper = NinjectProvider.Get<IMapper>();
    }

    public async Task<IActionResult> Index(){
        var models = await _vmodelRepo.List();
        IEnumerable<VehicleMakeModelVM> list = _mapper.Map<IEnumerable<VehicleModel>, IEnumerable<VehicleMakeModelVM>>(models);

        return View(list);
    }

    public async Task<IActionResult> Manufacturers(){
        var models = await _vmakeRepo.List();
        IEnumerable<ManufacturersVM> list = _mapper.Map<IEnumerable<VehicleMake>, IEnumerable<ManufacturersVM>>(models);

        return View(list);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string name){
        var models = await _vmakeRepo.Create(name);
        IEnumerable<ManufacturersVM> list = _mapper.Map<IEnumerable<VehicleMake>, IEnumerable<ManufacturersVM>>(models);

        return RedirectToAction(nameof(Manufacturers));
    }



    [HttpPost]
    public async Task<IActionResult> Delete(int id){
        var models = await _vmakeRepo.Delete(id);

        return RedirectToAction(nameof(Manufacturers));
    }

    public async Task<IActionResult> Manufacturer(int id){
        var manufacturer = await _vmakeRepo.Get(id);
        IEnumerable<VehicleModel> models = await _vmodelRepo.List();

        ManufacturerVM model = EntityMapper.Map<ManufacturerVM>(manufacturer, models);

        return View(model);
    }
}