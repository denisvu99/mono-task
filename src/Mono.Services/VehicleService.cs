using System.Collections.Concurrent;
using AutoMapper;
using Mono.Contracts;
using Mono.Contracts.Models;
using Mono.Contracts.Repositories;
using Mono.Contracts.Services;
using Mono.Data;

namespace Mono.Services;

public class VehicleService : IVehicleService
{
    private IVehicleMakeRepository _vmakeRepo;
    private IVehicleModelRepository _vmodelRepo;
    private static ConcurrentDictionary<int, VehicleMake>? manufacturerCache;
    private static ConcurrentDictionary<int, VehicleModel>? vehicleModelCache;

    public VehicleService(){
        NinjectProvider.Initialize();
        var db = NinjectProvider.Get<AppDbContext>();
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        _vmakeRepo = NinjectProvider.Get<IVehicleMakeRepository>();
        _vmodelRepo = NinjectProvider.Get<IVehicleModelRepository>();


        manufacturerCache ??= _vmakeRepo.InitDictionary();
        vehicleModelCache ??= _vmodelRepo.InitDictionary();
    }

    public async Task<IEnumerable<ManufacturersVM>> ListManufacturers()
    {
        IEnumerable<ManufacturersVM> list;
        if(manufacturerCache != null){
            list = EntityMapper.Map<IEnumerable<ManufacturersVM>>(manufacturerCache.Values);
        }else{
            manufacturerCache = await _vmakeRepo.List();
            list = EntityMapper.Map<IEnumerable<ManufacturersVM>>(manufacturerCache.Values);
        }
        
        return list;
    }

    public async Task<ManufacturerVM?> GetManufacturer(int id)
    {
        if(manufacturerCache == null || vehicleModelCache == null) return null;

        manufacturerCache.TryGetValue(id, out VehicleMake? manufacturer);
        if (manufacturer == null){
            manufacturer ??= await _vmakeRepo.Get(id);
            if (manufacturer != null) manufacturerCache.AddOrUpdate(id, manufacturer, (key, oldVal) => manufacturer);
            else return null;
        }

        IEnumerable<VehicleModel> models = (IEnumerable<VehicleModel>)vehicleModelCache.Values;

        ManufacturerVM model = EntityMapper.Map<ManufacturerVM>(manufacturer, vehicleModelCache.Values);

        return model;
    }

    public async Task<IEnumerable<ManufacturersVM>> CreateManufacturer(string name)
    {
        VehicleMake model = new VehicleMake() {ManufacturerName = name};
        var manufacturer = await _vmakeRepo.Create(model);
        if (manufacturer != null) {
            if(manufacturerCache == null) return null;
            manufacturerCache.AddOrUpdate(manufacturer.VehicleMakeId, manufacturer, (key, oldVal) => manufacturer);
        }
        if(manufacturerCache == null) return null;
        return EntityMapper.Map<IEnumerable<ManufacturersVM>>(manufacturerCache.Values);
    }

    public async Task<ManufacturerVM?> UpdateManufacturerName(int id, string name)
    {
        bool affected = await _vmakeRepo.UpdateName(id, name);
        if (affected) {
            if(manufacturerCache != null){
                if(manufacturerCache.TryGetValue(id, out VehicleMake? model)){
                    model.ManufacturerName = name;
                    if(manufacturerCache.TryUpdate(id, model, model))
                    return EntityMapper.Map<ManufacturerVM>(model);
                }
            }
        }
        return null;
    }

    public async Task<bool?> DeleteManufacturer(int id)
    {
        bool? affected = await _vmakeRepo.Delete(id);
        if (affected == true) {
            if (manufacturerCache == null) return null;

            return manufacturerCache.TryRemove(id, out _);
        }

        return affected;
    }

    public async Task<bool?> AddModelToManufacturer(int manufacturerId, int modelId)
    {
        bool? affected = await _vmodelRepo.AddTo(manufacturerId, modelId);
        if( affected == true){
            if(vehicleModelCache != null && manufacturerCache != null){
                if (vehicleModelCache.TryGetValue(modelId, out VehicleModel? vehicleModel) &&
                manufacturerCache.TryGetValue(manufacturerId, out VehicleMake? manufacturer))
                {
                    vehicleModel.VehicleMake = manufacturer;
                    vehicleModel.VehicleMakeId = manufacturer.VehicleMakeId;
                    vehicleModelCache.TryUpdate(modelId, vehicleModel, vehicleModel);
                    manufacturer.VehicleModels.Add(vehicleModel);
                    manufacturerCache.TryUpdate(manufacturerId, manufacturer, manufacturer);
                }

            }
        }
        
        return affected;
    }

    public async Task<bool?> RemoveModelFromManufacturer(int manufacturerId,int modelId)
    {
        bool? affected = await _vmodelRepo.RemoveFrom(modelId);
        if (affected == true){
            if (vehicleModelCache != null && manufacturerCache != null){
                if (vehicleModelCache.TryGetValue(modelId, out VehicleModel? model) &&
                    manufacturerCache.TryGetValue(manufacturerId, out VehicleMake? manufacturer)){
                    
                    manufacturer.VehicleModels.Remove(model);
                    manufacturerCache.TryUpdate(manufacturerId, manufacturer, manufacturer);
                    
                    model.VehicleMakeId = null;
                    model.VehicleMake = null;
                    vehicleModelCache.TryUpdate(modelId, model, model);
                }
            }
        }
        
        return affected;
    }

    public async Task<VehicleModelsExtendedVM> ListVehicleModels()
    {
        IEnumerable<VehicleModel> vehicleModels;
        if (vehicleModelCache != null){
            vehicleModels = vehicleModelCache.Values;
        }else{
            vehicleModelCache  = await _vmodelRepo.List();
            vehicleModels = vehicleModelCache.Values;
        }
        IEnumerable<VehicleMake> vehicleManufacturers;
        if (manufacturerCache != null){
            vehicleManufacturers = manufacturerCache.Values;
        }else{
            manufacturerCache  = await _vmakeRepo.List();
            vehicleManufacturers = manufacturerCache.Values;
        }
        IEnumerable<VehicleModelVM> vModelsList = EntityMapper.Map<IEnumerable<VehicleModelVM>>(vehicleModels);
        VehicleModelsExtendedVM viewModel = EntityMapper.Map<VehicleModelsExtendedVM>(vModelsList, vehicleManufacturers);
        

        return viewModel;
    }

    public async Task<UpdateVehicleModelVM> GetVehicleModel(int id)
    {

        var model = await _vmodelRepo.Get(id);
        var manufacturers = await _vmakeRepo.List();
        var vModel = EntityMapper.Map<UpdateVehicleModelVM>(model, manufacturers);

        throw new NotImplementedException();
    }

    public async Task<bool?> CreateVehicleModel(CreateVehicleModelVM viewModel)
    {
        var model = EntityMapper.Map<VehicleModel>(viewModel);
        await _vmodelRepo.Create(model);

        throw new NotImplementedException();
    }

    public async Task<bool?> UpdateVehicleModel(UpdateVehicleModelVM viewModel)
    {
        var model = EntityMapper.Map<VehicleModel>(viewModel);
        await _vmodelRepo.Update(model); 

        throw new NotImplementedException();
    }
}