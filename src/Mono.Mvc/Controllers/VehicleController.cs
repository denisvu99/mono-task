using Microsoft.AspNetCore.Mvc;
using Mono.Contracts.Repositories;

namespace Mono.Mvc.Controllers;

public class VehicleController : Controller
{
    private readonly ILogger<VehicleController> _logger;
    private IVehicleMakeRepository _vmakeRepo;
    private IVehicleModelRepository _vmodelRepo;

    public VehicleController(ILogger<VehicleController> logger)
    {
        _logger = logger;
        _vmakeRepo = NinjectProvider.Get<IVehicleMakeRepository>();
        _vmodelRepo = NinjectProvider.Get<IVehicleModelRepository>();
    }

    // public async Task<IActionResult> Index(){
    //     var model = _vmakeRepo.List();
    // }
}