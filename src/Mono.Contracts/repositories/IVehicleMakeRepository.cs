using Mono.Contracts.Models;

namespace Mono.Contracts.Repositories;
public interface IVehicleMakeRepository {
    Task<VehicleMake?> Create(VehicleMake model);
    public Task<IEnumerable<VehicleMake>> List();
    Task<VehicleMake?> Get(int id);
    Task<VehicleMake?> Update(int id, VehicleMake model);
    Task<bool?> Delete(int id);
}