using Microsoft.EntityFrameworkCore.ChangeTracking;
using Mono.Contracts.Models;
using Mono.Contracts.Repositories;
using Mono.Data;
using System.Collections.Concurrent;

public class VehicleMakeRepository : IVehicleMakeRepository
{
    private AppDbContext _db;
    private static ConcurrentDictionary<int, VehicleMake>? vehicleMakeCache;

    public VehicleMakeRepository(AppDbContext db){
        _db = db;

        if (vehicleMakeCache is null)
        {
            vehicleMakeCache = new ConcurrentDictionary<int, VehicleMake>(_db.VehicleMakes.ToDictionary(vm => vm.VehicleMakeId));
        }
    }
    public async Task<VehicleMake?> Create(VehicleMake model)
    {
        EntityEntry<VehicleMake> vm = await _db.VehicleMakes.AddAsync(model);
        int affected = await _db.SaveChangesAsync();
        if (affected == 1){
            if (vehicleMakeCache == null) return model;

            return vehicleMakeCache.AddOrUpdate(model.VehicleMakeId,model, (key, oldVal) => model);
        }

        return null;
    }
    public async Task<VehicleMake?> Get(int id)
    {
        if (vehicleMakeCache == null) return null;
        vehicleMakeCache.TryGetValue(id, out VehicleMake? vm);
        return await Task.FromResult(vm);
    }

    public async Task<IEnumerable<VehicleMake>> List()
    {
        return await Task.FromResult(vehicleMakeCache == null ? Enumerable.Empty<VehicleMake>() : vehicleMakeCache.Values);
    }

    public async Task<VehicleMake?> Update(int id, VehicleMake model)
    {
        VehicleMake? old;
        if(vehicleMakeCache != null){
            if(vehicleMakeCache.TryGetValue(id, out old)){
                if(vehicleMakeCache.TryUpdate(id, model, old))
                return await Task.FromResult(model);
            }
        }
        return null;
    }


    public async Task<bool?> Delete(int id)
    {
        VehicleMake? vm = _db.VehicleMakes.Find(id);
        if(vm != null){
            _db.VehicleMakes.Remove(vm);
            int affected = await _db.SaveChangesAsync();
            if(affected == 1){
                if(vehicleMakeCache == null) return null;

                return vehicleMakeCache.TryRemove(id, out vm);
            }
        }
        return null;
    }

}