using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Mono.Contracts.Models;
using Mono.Contracts.Repositories;
using Mono.Data;
using System.Collections.Concurrent;

public class VehicleModelRepository : IVehicleModelRepository
{
    private AppDbContext _db;
    private static ConcurrentDictionary<int, VehicleModel>? vehicleModelCache;

    public VehicleModelRepository(){
        _db = NinjectProvider.Get<AppDbContext>();

        if (vehicleModelCache is null)
        {
            vehicleModelCache = new ConcurrentDictionary<int, VehicleModel>(_db.VehicleModels.ToDictionary(vm => vm.VehicleModelId));
        }
    }
    public async Task<VehicleModel?> Create(VehicleModel model)
    {
        EntityEntry<VehicleModel> vm = await _db.VehicleModels.AddAsync(model);
        int affected = await _db.SaveChangesAsync();
        if (affected == 1){
            if (vehicleModelCache == null) return model;

            return vehicleModelCache.AddOrUpdate(model.VehicleModelId,model, (key, oldVal) => model);
        }

        return null;
    }
    public async Task<VehicleModel?> Get(int id)
    {
        if (vehicleModelCache == null) return null;
        vehicleModelCache.TryGetValue(id, out VehicleModel? vm);
        return await Task.FromResult(vm);
    }

    public async Task<IEnumerable<VehicleModel>> List()
    {
        return await Task.FromResult(vehicleModelCache == null ? Enumerable.Empty<VehicleModel>() : vehicleModelCache.Values);
    }

    public async Task<VehicleModel?> Update(int id, VehicleModel model)
    {
        VehicleModel? old;
        if(vehicleModelCache != null){
            if(vehicleModelCache.TryGetValue(id, out old)){
                if(vehicleModelCache.TryUpdate(id, model, old))
                return await Task.FromResult(model);
            }
        }
        return null;
    }


    public async Task<bool?> Delete(int id)
    {
        VehicleModel? vm = _db.VehicleModels.Find(id);
        if(vm != null){
            _db.VehicleModels.Remove(vm);
            int affected = await _db.SaveChangesAsync();
            if(affected >= 1){
                if(vehicleModelCache == null) return null;

                return vehicleModelCache.TryRemove(id, out vm);
            }
        }
        return null;
    }

    public async Task<VehicleModel?> AddTo(int manufacturerId, int modelId){

        var affected = _db.VehicleModels
            .Where(vm => vm.VehicleModelId == modelId)
            .ExecuteUpdateAsync(e => e.SetProperty(p => p.VehicleMakeId, manufacturerId));

        VehicleModel? old;
        if(vehicleModelCache != null){
            if(vehicleModelCache.TryGetValue(modelId, out old)){
                old.VehicleMakeId = manufacturerId;
                if(vehicleModelCache.TryUpdate(modelId, old, old)){
                    return await Task.FromResult(old);
                }
            }
        }
        return null;
    }

    public async Task<VehicleModel?> RemoveFrom(int modelId){

        var affected = _db.VehicleModels
            .Where(vm => vm.VehicleModelId == modelId)
            .ExecuteUpdateAsync(e => e.SetProperty(p => p.VehicleMakeId, (int?)null));

        VehicleModel? old;
        if(vehicleModelCache != null){
            if(vehicleModelCache.TryGetValue(modelId, out old)){
                old.VehicleMakeId = null;
                old.VehicleMake = null;
                if(vehicleModelCache.TryUpdate(modelId, old, old)){
                    return await Task.FromResult(old);
                }
            }
        }
        return null;
    }
}