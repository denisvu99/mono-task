using System.Collections.Concurrent;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

    public VehicleService(){
        NinjectProvider.Initialize();
        var db = NinjectProvider.Get<AppDbContext>();
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        _vmakeRepo = NinjectProvider.Get<IVehicleMakeRepository>();
        _vmodelRepo = NinjectProvider.Get<IVehicleModelRepository>();
    }

    public async Task<IEnumerable<ManufacturersVM>> ListManufacturers(string sortOrder, int? filter)
    {
        var viewModel = EntityMapper.Map<IEnumerable<ManufacturersVM>>(await _vmakeRepo.List());

        if (filter != null){
            viewModel = viewModel.Where(p => p.Count == filter);
        }

        viewModel = sortOrder switch
        {
            "man_desc" => viewModel.OrderByDescending(s => s.ManufacturerName),
            "count_desc" => viewModel.OrderByDescending(s => s.Count),
            "count_asc" => viewModel.OrderBy(s => s.Count),
            _ => viewModel.OrderBy(s => s.ManufacturerName),
        };
        return viewModel;
    }

    public async Task<ManufacturerVM?> GetManufacturer(int id)
    {
        var manufacturer = await _vmakeRepo.Get(id);
        if(manufacturer == null)return null;
        IEnumerable<VehicleModel> models = await _vmodelRepo.List();

        return EntityMapper.Map<ManufacturerVM>(manufacturer, models);
    }

    public async Task<bool?> CreateManufacturer(string name)
    {
        return await _vmakeRepo.Create(new VehicleMake() {ManufacturerName = name});
    }

    public async Task<bool?> UpdateManufacturerName(int id, string name)
    {
        return await _vmakeRepo.UpdateName(id, name);
    }

    public async Task<bool?> DeleteManufacturer(int id)
    {
        return await _vmakeRepo.Delete(id);
    }

    public async Task<bool?> AddModelToManufacturer(int manufacturerId, int modelId)
    {
        return await _vmodelRepo.AddTo(manufacturerId, modelId);
    }

    public async Task<bool?> RemoveModelFromManufacturer(int manufacturerId,int modelId)
    {
        return await _vmodelRepo.RemoveFrom(modelId);
    }

    public async Task<IEnumerable<VehicleModelVM>> ListVehicleModels(string sortOrder, int? filter)
    {
        IEnumerable<VehicleModelVM> vModelsList = EntityMapper.Map<IEnumerable<VehicleModelVM>>(await _vmodelRepo.List());

        if (filter != null){
            vModelsList = vModelsList.Where(p => p.ManufacturerId == filter);
        }

        vModelsList = sortOrder switch{
            "man_desc" => vModelsList.OrderByDescending(s => s.ManufacturerName),
            "model_desc" => vModelsList.OrderByDescending(s => s.ModelName),
            "model_asc" => vModelsList.OrderBy(s => s.ModelName),
            _ => vModelsList.OrderBy(s => s.ManufacturerName),
        };

        return vModelsList;
    }

    public async Task<UpdateVehicleModelVM?> GetVehicleModel(int id)
    {
        var vehicleModel = await _vmodelRepo.Get(id);
        if (vehicleModel == null) return null;
        var manufacturers = await _vmakeRepo.List();

        return EntityMapper.Map<UpdateVehicleModelVM>(vehicleModel, manufacturers);
    }

    public async Task<bool?> CreateVehicleModel(CreateVehicleModelVM viewModel)
    {
        return await _vmodelRepo.Create(EntityMapper.Map<VehicleModel>(viewModel));
    }

    public async Task<bool?> UpdateVehicleModel(UpdateVehicleModelVM viewModel)
    {
        return await _vmodelRepo.Update(EntityMapper.Map<VehicleModel>(viewModel)); 
    }

    public async Task<bool?> DeleteVehicleModel(int id){
        return await _vmodelRepo.Delete(id);
    }

    public async Task<IEnumerable<ViewBagManufacturer>> ListViewBagManufacturers()
    {
        return  EntityMapper.Map<IEnumerable<ViewBagManufacturer>>(await _vmakeRepo.List());
    }
}