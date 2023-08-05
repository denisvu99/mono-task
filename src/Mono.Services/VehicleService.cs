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
    private IMapper _mapper;
    private static ConcurrentDictionary<int, VehicleMake>? vehicleMakeCache;

    public VehicleService(){
        NinjectProvider.Initialize();
        var db = NinjectProvider.Get<AppDbContext>();
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
        _vmakeRepo = NinjectProvider.Get<IVehicleMakeRepository>();
        _vmodelRepo = NinjectProvider.Get<IVehicleModelRepository>();
        _mapper = NinjectProvider.Get<IMapper>();


        if (vehicleMakeCache is null)
        {
            vehicleMakeCache = _vmakeRepo.InitDictionary();
        }
    }

    public async Task<IEnumerable<ManufacturersVM>> ListManufacturers()
    {
        IEnumerable<ManufacturersVM> list;
        if(vehicleMakeCache != null){
            list = _mapper.Map<IEnumerable<VehicleMake>, IEnumerable<ManufacturersVM>>(vehicleMakeCache.Values);
        }else{
            var models = await _vmakeRepo.List();
            list = _mapper.Map<IEnumerable<VehicleMake>, IEnumerable<ManufacturersVM>>(models.Values);
        }
        
        return list;
    }

    public async Task<ManufacturerVM?> GetManufacturer(int id)
    {
        VehicleMake? manufacturer;
        if(vehicleMakeCache == null) return null;

        vehicleMakeCache.TryGetValue(id, out manufacturer);
        if (manufacturer == null){
            manufacturer ??= await _vmakeRepo.Get(id);
            if (manufacturer != null) vehicleMakeCache.AddOrUpdate(id, manufacturer, (key, oldVal) => manufacturer);
            else return null;
        }

        IEnumerable<VehicleModel> models = await _vmodelRepo.List();

        ManufacturerVM model = EntityMapper.Map<ManufacturerVM>(manufacturer, models);

        return model;
    }

    public async Task<IEnumerable<ManufacturersVM>> CreateManufacturer(string name)
    {
        VehicleMake model = new VehicleMake() {ManufacturerName = name};
        var manufacturer = await _vmakeRepo.Create(model);
        if (manufacturer != null) {
            if(vehicleMakeCache == null) return null;
            vehicleMakeCache.AddOrUpdate(manufacturer.VehicleMakeId, manufacturer, (key, oldVal) => manufacturer);
        }
        if(vehicleMakeCache == null) return null;
        return _mapper.Map<IEnumerable<VehicleMake>, IEnumerable<ManufacturersVM>>(vehicleMakeCache.Values);
    }

    public async Task<ManufacturerVM?> UpdateManufacturerName(int id, string name)
    {
        bool affected = await _vmakeRepo.UpdateName(id, name);
        if (affected) {
            VehicleMake? model;
            if(vehicleMakeCache != null){
                if(vehicleMakeCache.TryGetValue(id, out model)){
                    model.ManufacturerName = name;
                    if(vehicleMakeCache.TryUpdate(id, model, model))
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
            if (vehicleMakeCache == null) return null;

            return vehicleMakeCache.TryRemove(id, out _);
        }

        return affected;
    }

    public async Task<bool?> AddModelToManufacturer(int manufacturerId, int modelId)
    {
        await _vmodelRepo.AddTo(manufacturerId, modelId);

        throw new NotImplementedException();
    }

    public async Task<bool?> RemoveModelFromManufacturer(int modelId)
    {
        await _vmodelRepo.RemoveFrom(modelId);

        throw new NotImplementedException();
    }

    public async Task<VehicleModelsExtendedVM> ListVehicleModels()
    {
        var models = await _vmodelRepo.List();
        var manufacturers = await _vmakeRepo.List();
        IEnumerable<VehicleModelVM> vModelsList = _mapper.Map<IEnumerable<VehicleModel>, IEnumerable<VehicleModelVM>>(models);
        VehicleModelsExtendedVM viewModel = EntityMapper.Map<VehicleModelsExtendedVM>(vModelsList, manufacturers);
        

        return null;
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