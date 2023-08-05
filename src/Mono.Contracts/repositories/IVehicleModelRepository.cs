using System.Collections.Concurrent;
using Mono.Contracts.Models;

namespace Mono.Contracts.Repositories;
public interface IVehicleModelRepository {
    ConcurrentDictionary<int,VehicleModel> InitDictionary();
    Task<VehicleModel?> Create(VehicleModel model);
    Task<ConcurrentDictionary<int, VehicleModel>> List();
    Task<VehicleModel?> Get(int id);
    Task<VehicleModel?> Update(VehicleModel model);
    Task<bool?> Delete(int id);
    Task<bool?> AddTo(int manufacturerId, int modelId);
    Task<bool?> RemoveFrom(int modelId);
}