
using Ninject;
using Ninject.Modules;
using AutoMapper;

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

            //     config.CreateMap<MySource, MyDest>();
            // .... other mappings, Profiles, etc.          
        });

        config.AssertConfigurationIsValid();

        return new Mapper(config);
    }
}