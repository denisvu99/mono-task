using Mono.Contracts.Models;

public class ManufacturerVM {
    public int VehicleMakeId {get; set;}
    public string ManufacturerName {get; set;}
    public ICollection<VehicleModel> AllVehicleModels {get; set;}
}