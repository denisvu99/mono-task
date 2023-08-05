using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Mono.Contracts.Models;
using Mono.Contracts.Repositories;
using Mono.Data;
using System.Collections.Concurrent;


public class VehicleMakeRepository : IVehicleMakeRepository
{
    private AppDbContext _db;

    public VehicleMakeRepository(AppDbContext db){
        _db = db;
    }

    public ConcurrentDictionary<int, VehicleMake> InitDictionary()
    {
        var dictionary = _db.VehicleMakes.Include(m => m.VehicleModels).ToDictionary(vm => vm.VehicleMakeId);

        return new ConcurrentDictionary<int, VehicleMake>(dictionary);
    }

    public async Task<ConcurrentDictionary<int, VehicleMake>> List()
    {
        var dictionary = await _db.VehicleMakes.Include(m => m.VehicleModels).ToDictionaryAsync(vm => vm.VehicleMakeId);

        return new ConcurrentDictionary<int, VehicleMake>(dictionary);
    }

    public async Task<VehicleMake?> Get(int id)
    {
        return await _db.VehicleMakes.Include(m => m.VehicleModels).FirstOrDefaultAsync(e => e.VehicleMakeId == id);
    }

    public async Task<VehicleMake?> Create(VehicleMake model)
    {
        var entity = await _db.VehicleMakes.AddAsync(model);
        var affected = await _db.SaveChangesAsync();
        if (affected == 1) return entity.Entity;
        return null;
    }

    public async Task<bool?> Delete(int id)
    {
        var manufacturer = await Get(id);
        if (manufacturer == null) return null;
        _db.VehicleMakes.Remove(manufacturer);
        var affected = await _db.SaveChangesAsync();
        return affected >= 1;
    }

    public async Task<bool> UpdateName(int id, string name)
    {
        var affected = await _db.VehicleMakes
            .Where(p => p.VehicleMakeId == id)
            .ExecuteUpdateAsync(e => e
                .SetProperty(p => p.ManufacturerName, name));
        return affected >= 1;
    }
}

// public class VehicleMakeRepository : IVehicleMakeRepository
// {
//     private AppDbContext _db;

//     public VehicleMakeRepository(AppDbContext db){
//         _db = NinjectProvider.Get<AppDbContext>();
//     }
//     public async Task<IEnumerable<VehicleMake>> Create(string name)
//     {
//         var model = new VehicleMake { ManufacturerName = name };
//         EntityEntry<VehicleMake> vm = await _db.VehicleMakes.AddAsync(model);
//         int affected = await _db.SaveChangesAsync();
//         if (affected == 1){
//             if (vehicleMakeCache == null) return await Task.FromResult(vehicleMakeCache == null ? Enumerable.Empty<VehicleMake>() : vehicleMakeCache.Values);;
//             vehicleMakeCache.AddOrUpdate(model.VehicleMakeId,model, (key, oldVal) => model);

//             return await Task.FromResult(vehicleMakeCache == null ? Enumerable.Empty<VehicleMake>() : vehicleMakeCache.Values);
//         }

//         return null;
//     }
//     public async Task<VehicleMake?> Get(int id)
//     {
//         if (vehicleMakeCache == null) return null;
//         vehicleMakeCache.TryGetValue(id, out VehicleMake? vm);
//         return await Task.FromResult(vm);
//     }

//     public async Task<ConcurrentDictionary<int,VehicleMake>> List()
//     {
//         return await _db.VehicleMakes.Include(m => m.VehicleModels).ToDictionaryAsync(vm => vm.VehicleMakeId);
//     }

//     public async Task<VehicleMake?> UpdateName(int id, string name)
//     {
//         var affected =await _db.VehicleMakes
//             .Where(vm => vm.VehicleMakeId == id)
//             .ExecuteUpdateAsync(e => e.SetProperty(p => p.ManufacturerName, name));

//         if (affected >= 1){
//             VehicleMake? old;
//             if(vehicleMakeCache != null){
//                 if(vehicleMakeCache.TryGetValue(id, out old)){
//                     old.ManufacturerName = name;
//                     if(vehicleMakeCache.TryUpdate(id, old, old))
//                     return await Task.FromResult(old);
//                 }
//             }
//         }
//         return null;
//     }


//     public async Task<bool?> Delete(int id)
//     {
//         VehicleMake? vm = _db.VehicleMakes.Find(id);
//         if(vm != null){
//             _db.VehicleMakes.Remove(vm);
//             int affected = await _db.SaveChangesAsync();
//             if(affected >= 1){
//                 if(vehicleMakeCache == null) return null;

//                 return vehicleMakeCache.TryRemove(id, out vm);
//             }
//         }
//         return null;
//     }

// }