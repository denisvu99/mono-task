using Ninject;
using Ninject.Modules;
using AutoMapper;
using Mono.Contracts.Models;

public class AutoMapperModule : NinjectModule
{
    public override void Load()
    {
        Bind<IMapper>().ToMethod(AutoMapper).InSingletonScope();
    }

    private IMapper AutoMapper(Ninject.Activation.IContext context)
    {
        var config = new MapperConfiguration(cfg => {
            cfg.ConstructServicesUsing(type => context.Kernel.Get(type));

            //Config ManufacturersVM
            cfg.CreateMap<VehicleMake, ManufacturersVM>()
                .ForMember(dest => dest.Count, opt => opt.MapFrom(src => Convert.ToInt64(src.VehicleModels.Count())));

            //Config ManufacturerVM
            cfg.CreateMap<VehicleMake, ManufacturerVM>()
                .ForMember(dest => dest.VehicleMakeId, opt => opt.MapFrom(s => s.VehicleMakeId))
                .ForMember(dest => dest.ManufacturerName, opt => opt.MapFrom(s => s.ManufacturerName))
                .ForMember(dest => dest.AllVehicleModels, opt => opt.Ignore());

            cfg.CreateMap<IEnumerable<VehicleModel>, ManufacturerVM>()
                .ForMember(dest => dest.VehicleMakeId, opt => opt.Ignore())
                .ForMember(dest => dest.ManufacturerName, opt => opt.Ignore())
                .ForMember(dest => dest.AllVehicleModels, opt => opt.MapFrom(s => s));
                
            //Config VehicleModelsVM
            cfg.CreateMap<VehicleModel, VehicleModelVM>()
                .ForMember(dest => dest.ModelId, opt => opt.MapFrom(s => s.VehicleModelId))
                .ForMember(dest => dest.ModelName, opt => opt.MapFrom(s => s.ModelName))
                .ForMember(dest => dest.ManufacturerId, opt => opt.MapFrom(s => s.VehicleMake.VehicleMakeId))
                .ForMember(dest => dest.ManufacturerName, opt => opt.MapFrom(s => s.VehicleMake.ManufacturerName));

            //Config VehicleModelsExtendedVM
            cfg.CreateMap<IEnumerable<VehicleMake>, VehicleModelsExtendedVM>()
                .ForMember(dest => dest.VehicleModels, opt => opt.Ignore())
                .ForMember(dest => dest.Manufacturers, opt => opt.MapFrom(s => s));

            cfg.CreateMap<IEnumerable<VehicleModelVM>, VehicleModelsExtendedVM>()
                .ForMember(dest => dest.VehicleModels, opt => opt.MapFrom(s => s))
                .ForMember(dest => dest.Manufacturers, opt => opt.Ignore());

            //Config CreateVehicleModelVM
            cfg.CreateMap<CreateVehicleModelVM, VehicleModel>()
                .ForMember(dest => dest.ModelName, opts => opts.MapFrom(s => s.Name))
                .ForMember(dest => dest.VehicleMakeId, opts => opts.MapFrom(s => s.ManufacturerId))
                .ForMember(dest => dest.VehicleModelId, opts => opts.Ignore())
                .ForMember(dest => dest.VehicleMake, opts => opts.Ignore());

            //Config UpdateVehicleModelVM
            cfg.CreateMap<VehicleModel, UpdateVehicleModelVM>()
                .ForMember(dest => dest.Name, opts => opts.MapFrom(s => s.ModelName))
                .ForMember(dest => dest.ModelId, opts => opts.MapFrom(s => s.VehicleModelId))
                .ForMember(dest => dest.ManufacturerId, opts => opts.MapFrom(s => s.VehicleMakeId))
                .ForMember(dest => dest.ManufacturersList, opts => opts.Ignore());

            cfg.CreateMap<IEnumerable<VehicleMake>, UpdateVehicleModelVM>()
                .ForMember(dest => dest.Name, opts => opts.Ignore())
                .ForMember(dest => dest.ModelId, opts => opts.Ignore())
                .ForMember(dest => dest.ManufacturerId, opts => opts.Ignore())
                .ForMember(dest => dest.ManufacturersList, opts => opts.MapFrom(s => s));

            cfg.CreateMap<UpdateVehicleModelVM, VehicleModel>()
                .ForMember(dest => dest.VehicleModelId, opts => opts.MapFrom(s => s.ModelId))
                .ForMember(dest => dest.ModelName, opts => opts.MapFrom(s => s.Name))
                .ForMember(dest => dest.VehicleMakeId, opts => opts.MapFrom(s => s.ManufacturerId))
                .ForMember(dest => dest.VehicleMake, opts => opts.Ignore());

        });
        config.AssertConfigurationIsValid();

        return new Mapper(config);
    }
}
