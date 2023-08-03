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

            cfg.CreateMap<VehicleMake, ManufacturersVM>()
                .ForMember(dest => dest.Count, opt => opt.MapFrom(src => Convert.ToInt64(src.VehicleModels.Count())))
            ;

            cfg.CreateMap<VehicleMake, VehicleMakeModelVM>()
                .ForMember(dest => dest.ManufacturerName, opt => opt.MapFrom(src => src.ManufacturerName))
                .ForMember(dest => dest.ModelName, opt => opt.MapFrom(src => ""));

            cfg.CreateMap<VehicleModel, VehicleMakeModelVM>()
                .ForMember(dest => dest.ManufacturerName, opt => opt.MapFrom(src => src.VehicleMake.ManufacturerName))
                .ForMember(dest => dest.ModelName, opt => opt.MapFrom(src => src.ModelName));
        });

        config.AssertConfigurationIsValid();

        return new Mapper(config);
    }
}
