using Mono.Contracts.Models;

namespace Mono.Contracts.Repositories;
public interface IVehicleModelRepository {
    Task<VehicleModel?> Create(VehicleModel model);
    Task<IEnumerable<VehicleModel>> List();
    Task<VehicleModel?> Get(int id);
    Task<VehicleModel?> Update(VehicleModel model);
    Task<bool?> Delete(int id);
    Task<VehicleModel?> AddTo(int manufacturerId, int modelId);
    Task<VehicleModel?> RemoveFrom(int modelId);
}