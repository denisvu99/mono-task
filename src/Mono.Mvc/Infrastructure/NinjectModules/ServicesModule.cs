using Mono.Contracts.Services;
using Mono.Services;
using Ninject.Modules;

internal class ServicesModule : NinjectModule
{
    public override void Load()
    {
        Bind<IAppDbInitializer>().To<AppDbInitializer>();
        Bind<IVehicleService>().To<VehicleService>();
    }
}