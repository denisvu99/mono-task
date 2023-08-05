using System.Collections.Concurrent;
using Mono.Contracts.Models;

namespace Mono.Contracts.Repositories;
public interface IVehicleMakeRepository {
    ConcurrentDictionary<int,VehicleMake> InitDictionary();
    Task<VehicleMake?> Create(VehicleMake model);
    Task<ConcurrentDictionary<int,VehicleMake>> List();
    Task<VehicleMake?> Get(int id);
    Task<bool> UpdateName(int id, string name);
    Task<bool> Delete(int id);
}