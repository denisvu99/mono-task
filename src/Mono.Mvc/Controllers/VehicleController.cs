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
        var manufacturers = await _vmakeRepo.List();
        IEnumerable<VehicleModelVM> vModelsList = _mapper.Map<IEnumerable<VehicleModel>, IEnumerable<VehicleModelVM>>(models);
        VehicleModelsExtendedVM viewModel = EntityMapper.Map<VehicleModelsExtendedVM>(vModelsList, manufacturers);

        return View(viewModel);
    }

    public async Task<IActionResult> Manufacturers(){
        var models = await _vmakeRepo.List();
        IEnumerable<ManufacturersVM> list = _mapper.Map<IEnumerable<VehicleMake>, IEnumerable<ManufacturersVM>>(models);

        return View(list);
    }

    [HttpPost]
    public async Task<IActionResult> CreateManufacturer(string name){
        var models = await _vmakeRepo.Create(name);
        IEnumerable<ManufacturersVM> list = _mapper.Map<IEnumerable<VehicleMake>, IEnumerable<ManufacturersVM>>(models);

        return RedirectToAction(nameof(Manufacturers));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteManufacturer(int id){
        await _vmakeRepo.Delete(id);

        return RedirectToAction(nameof(Manufacturers));
    }

    public async Task<IActionResult> Manufacturer(int id){
        var manufacturer = await _vmakeRepo.Get(id);
        IEnumerable<VehicleModel> models = await _vmodelRepo.List();

        ManufacturerVM model = EntityMapper.Map<ManufacturerVM>(manufacturer, models);

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateManufacturerName(int id, string name){
        await _vmakeRepo.UpdateName(id, name);

        return RedirectToAction(nameof(Manufacturer), new {id = id});
    }

    [HttpPost]
    public async Task<IActionResult> AddModelToManufacturer(int id, int modelId){
        await _vmodelRepo.AddTo(id, modelId);
        
        return RedirectToAction(nameof(Manufacturer), new {id = id});
    }

    [HttpPost]
    public async Task<IActionResult> RemoveModelFromManufacturer(int id, int modelId){
        await _vmodelRepo.RemoveFrom(modelId);

        return RedirectToAction(nameof(Manufacturer), new {id = id});
    }

    public async Task<IActionResult> VehicleModel(int id){
        var model = await _vmodelRepo.Get(id);
        var manufacturers = await _vmakeRepo.List();
        var vModel = EntityMapper.Map<UpdateVehicleModelVM>(model, manufacturers);

        return View(vModel);
    }

    [HttpPost]
    public async Task<IActionResult> CreateVehicleModel(CreateVehicleModelVM viewModel){
        var model = EntityMapper.Map<VehicleModel>(viewModel);
        await _vmodelRepo.Create(model);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> UpdateVehicleModel(UpdateVehicleModelVM viewModel){
        var model = EntityMapper.Map<VehicleModel>(viewModel);
        await _vmodelRepo.Update(model); 

        return RedirectToAction(nameof(Index));
    }


}