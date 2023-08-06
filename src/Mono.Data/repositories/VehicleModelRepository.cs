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

    public async Task<IEnumerable<VehicleModel>> List()
    {
        return await _db.VehicleModels.Include(p => p.VehicleMake).ToListAsync();
    }

    public async Task<bool?> Create(VehicleModel model)
    {
        await _db.VehicleModels.AddAsync(model);
        var affected = await _db.SaveChangesAsync();
        return affected == 1;
    }
    public async Task<VehicleModel?> Get(int id)
    {
        return await _db.VehicleModels.Include(e => e.VehicleMake).FirstOrDefaultAsync(p => p.VehicleModelId == id);
    }

    public async Task<bool> Update(VehicleModel model)
    {
        var affected = await _db.VehicleModels
            .Where(vm => vm.VehicleMakeId == model.VehicleMakeId)
            .ExecuteUpdateAsync(e => e
                .SetProperty(p => p.ModelName, model.ModelName)
                .SetProperty(p => p.VehicleMakeId, model.VehicleMakeId));
        
        return affected >= 1;
    }


    public async Task<bool?> Delete(int id)
    {
        var vehicleModel = await Get(id);
        if(vehicleModel == null) return null;
        _db.VehicleModels.Remove(vehicleModel);
        int affected = await _db.SaveChangesAsync();
        return affected >= 1;
    }

    public async Task<bool?> AddTo(int manufacturerId, int modelId){

        var model = await Get(modelId);
        if (model == null) return null;
        model.VehicleMakeId = manufacturerId;

        _db.VehicleModels.Update(model);
        var affected = await _db.SaveChangesAsync();

        return affected >= 1;
    }

    public async Task<bool?> RemoveFrom(int modelId){

        var model = await Get(modelId);
        if (model == null) return null;
        model.VehicleMakeId = null;
        _db.VehicleModels.Update(model);
        var affected = await _db.SaveChangesAsync();

        return affected >= 1;
    }
}