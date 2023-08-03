using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Mono.Contracts.Models;
using Mono.Contracts.Repositories;
using Mono.Data;
using System.Collections.Concurrent;


public class VehicleMakeRepository : IVehicleMakeRepository
{
    private AppDbContext _db;
    private static ConcurrentDictionary<int, VehicleMake>? vehicleMakeCache;

    public VehicleMakeRepository(AppDbContext db){
        _db = NinjectProvider.Get<AppDbContext>();

        if (vehicleMakeCache is null)
        {
            vehicleMakeCache = new ConcurrentDictionary<int, VehicleMake>(_db.VehicleMakes.Include(m => m.VehicleModels).ToDictionary(vm => vm.VehicleMakeId));
        }
    }
    public async Task<IEnumerable<VehicleMake>> Create(string name)
    {
        var model = new VehicleMake { ManufacturerName = name };
        EntityEntry<VehicleMake> vm = await _db.VehicleMakes.AddAsync(model);
        int affected = await _db.SaveChangesAsync();
        if (affected == 1){
            if (vehicleMakeCache == null) return await Task.FromResult(vehicleMakeCache == null ? Enumerable.Empty<VehicleMake>() : vehicleMakeCache.Values);;
            vehicleMakeCache.AddOrUpdate(model.VehicleMakeId,model, (key, oldVal) => model);

            return await Task.FromResult(vehicleMakeCache == null ? Enumerable.Empty<VehicleMake>() : vehicleMakeCache.Values);
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

    public async Task<VehicleMake?> UpdateName(int id, string name)
    {
        var affected =await _db.VehicleMakes
            .Where(vm => vm.VehicleMakeId == id)
            .ExecuteUpdateAsync(e => e.SetProperty(p => p.ManufacturerName, name));

        if (affected >= 1){
            VehicleMake? old;
            if(vehicleMakeCache != null){
                if(vehicleMakeCache.TryGetValue(id, out old)){
                    old.ManufacturerName = name;
                    if(vehicleMakeCache.TryUpdate(id, old, old))
                    return await Task.FromResult(old);
                }
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
            if(affected >= 1){
                if(vehicleMakeCache == null) return null;

                return vehicleMakeCache.TryRemove(id, out vm);
            }
        }
        return null;
    }

}