using System.Collections.Concurrent;
using Mono.Contracts.Models;

namespace Mono.Contracts.Repositories;
public interface IVehicleModelRepository {
    Task<bool?> Create(VehicleModel model);
    Task<IEnumerable<VehicleModel>> List();
    Task<VehicleModel?> Get(int id);
    Task<bool> Update(VehicleModel model);
    Task<bool?> Delete(int id);
    Task<bool?> AddTo(int manufacturerId, int modelId);
    Task<bool?> RemoveFrom(int modelId);
}