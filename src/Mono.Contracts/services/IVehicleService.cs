using Mono.Contracts.Models;

namespace Mono.Contracts.Services;
public interface IVehicleService {
    Task<IEnumerable<ManufacturersVM>> ListManufacturers();
    Task<ManufacturerVM?> GetManufacturer(int id);
    Task<IEnumerable<ManufacturersVM>> CreateManufacturer(string name);
    Task<bool?> DeleteManufacturer(int id);
    Task<ManufacturerVM?> UpdateManufacturerName(int id, string name);
    Task<bool?> AddModelToManufacturer(int manufacturerId, int modelId);
    Task<bool?> RemoveModelFromManufacturer(int manufacturerId, int modelId);
    Task<VehicleModelsExtendedVM> ListVehicleModels();
    Task<UpdateVehicleModelVM> GetVehicleModel(int id);
    Task<bool?> CreateVehicleModel(CreateVehicleModelVM viewModel);
    Task<bool?> UpdateVehicleModel(UpdateVehicleModelVM viewModel);
}