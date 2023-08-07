using System.Collections.Concurrent;
using Mono.Contracts.Models;

namespace Mono.Contracts.Repositories;
public interface IVehicleMakeRepository {
    Task<bool?> Create(VehicleMake model);
    Task<IEnumerable<VehicleMake>> List();
    Task<VehicleMake?> Get(int id);
    Task<bool?> UpdateName(int id, string name);
    Task<bool?> Delete(int id);
}