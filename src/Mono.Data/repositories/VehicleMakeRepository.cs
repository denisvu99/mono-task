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

    public async Task<IEnumerable<VehicleMake>> List()
    {
        return await _db.VehicleMakes.Include(e => e.VehicleModels).ToListAsync();
    }

    public async Task<VehicleMake?> Get(int id)
    {
        return await _db.VehicleMakes.Include(e => e.VehicleModels).FirstOrDefaultAsync(p => p.VehicleMakeId == id);
    }

    public async Task<bool?> Create(VehicleMake model)
    {
        await _db.VehicleMakes.AddAsync(model);
        var affected = await _db.SaveChangesAsync();
        return affected == 1;
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