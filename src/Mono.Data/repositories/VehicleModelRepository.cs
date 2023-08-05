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

    public VehicleModelRepository(AppDbContext db){
        _db = db;
    }

    public ConcurrentDictionary<int, VehicleModel> InitDictionary()
    {
        var dictionary = _db.VehicleModels.Include(p => p.VehicleMake).ToDictionary(vm => vm.VehicleModelId);

        return new ConcurrentDictionary<int, VehicleModel>(dictionary);
    }

    public async Task<ConcurrentDictionary<int, VehicleModel>> List()
    {
        var dictionary = await _db.VehicleModels.Include(p => p.VehicleMake).ToDictionaryAsync(vm => vm.VehicleModelId);

        return new ConcurrentDictionary<int, VehicleModel>(dictionary);
    }

    public async Task<VehicleModel?> Create(VehicleModel model)
    {
        var entity = await _db.VehicleModels.AddAsync(model);
        var affected = await _db.SaveChangesAsync();
        if (affected == 1) return entity.Entity;
        return null;
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

    public async Task<VehicleModel?> Update(VehicleModel model)
    {
        var affected = await _db.VehicleModels
            .Where(vm => vm.VehicleMakeId == model.VehicleMakeId)
            .ExecuteUpdateAsync(e => e
                .SetProperty(p => p.ModelName, model.ModelName)
                .SetProperty(p => p.VehicleMakeId, model.VehicleMakeId));

        if (affected >= 1){
            VehicleModel? old;
            if(vehicleModelCache != null){
                if(vehicleModelCache.TryGetValue(model.VehicleModelId, out old)){
                    old.ModelName = model.ModelName;
                    old.VehicleMakeId = model.VehicleMakeId;
                    if(vehicleModelCache.TryUpdate(model.VehicleModelId, old, old))
                    return await Task.FromResult(old);
                }
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

    public async Task<bool?> AddTo(int manufacturerId, int modelId){

        var affected = await _db.VehicleModels
            .Where(vm => vm.VehicleModelId == modelId)
            .ExecuteUpdateAsync(e => e.SetProperty(p => p.VehicleMakeId, manufacturerId));

        return affected >= 1;
    }

    public async Task<bool?> RemoveFrom(int modelId){

        var affected = await _db.VehicleModels
            .Where(vm => vm.VehicleModelId == modelId)
            .ExecuteUpdateAsync(e => e.SetProperty(p => p.VehicleMakeId, (int?)null));

        return affected >= 1;
    }
}