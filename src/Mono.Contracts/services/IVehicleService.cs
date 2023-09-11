using Mono.Contracts.Models;

namespace Mono.Contracts.Services;
public interface IVehicleService {
    Task<IEnumerable<ManufacturersVM>> ListManufacturers(string sortOrder, int? filter);
    Task<ManufacturerVM?> GetManufacturer(int id);
    Task<bool> CreateManufacturer(string name);
    Task<bool?> DeleteManufacturer(int id);
    Task<bool?> UpdateManufacturerName(int id, string name);
    Task<bool?> AddModelToManufacturer(int manufacturerId, int modelId);
    Task<bool?> RemoveModelFromManufacturer(int manufacturerId, int modelId);
    Task<IEnumerable<VehicleModelVM>> ListVehicleModels(string sortOrder, int? filter);
    Task<UpdateVehicleModelVM?> GetVehicleModel(int id);
    Task<bool> CreateVehicleModel(CreateVehicleModelVM viewModel);
    Task<bool?> UpdateVehicleModel(UpdateVehicleModelVM viewModel);
    Task<bool?> DeleteVehicleModel(int id);
    Task<IEnumerable<ViewBagManufacturer>> ListViewBagManufacturers ();
}