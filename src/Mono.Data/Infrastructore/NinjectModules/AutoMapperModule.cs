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
                .ForMember(dest => dest.ManufacturerName, opt => opt.MapFrom(s => s.VehicleMake.ManufacturerName))
                .ForMember(dest => dest.ModelName, opt => opt.MapFrom(s => s.ModelName));

            //Config VehicleModelsExtendedVM
            cfg.CreateMap<IEnumerable<VehicleMake>, VehicleModelsExtendedVM>()
                .ForMember(dest => dest.VehicleModels, opt => opt.Ignore())
                .ForMember(dest => dest.Manufacturers, opt => opt.MapFrom(s => s));

            cfg.CreateMap<IEnumerable<VehicleModelVM>, VehicleModelsExtendedVM>()
                .ForMember(dest => dest.VehicleModels, opt => opt.MapFrom(s => s))
                .ForMember(dest => dest.Manufacturers, opt => opt.Ignore());

        });
        config.AssertConfigurationIsValid();

        return new Mapper(config);
    }
}
