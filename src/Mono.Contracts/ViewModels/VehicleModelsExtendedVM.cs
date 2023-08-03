namespace Mono.Contracts.Models;

public class VehicleModelsExtendedVM {
    public ICollection<VehicleModelVM> VehicleModels {get; set;}
    public ICollection<VehicleMake> Manufacturers {get; set;}
}