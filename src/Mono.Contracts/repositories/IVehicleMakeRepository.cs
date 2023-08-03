using Mono.Contracts.Models;

namespace Mono.Contracts.Repositories;
public interface IVehicleMakeRepository {
    public Task<IEnumerable<VehicleMake>> Create(string name);
    public Task<IEnumerable<VehicleMake>> List();
    Task<VehicleMake?> Get(int id);
    Task<VehicleMake?> Update(int id, VehicleMake model);
    public Task<bool?> Delete(int id);
}