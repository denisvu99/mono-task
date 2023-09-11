using Mono.Contracts.Services;
using Mono.Services;
using Ninject.Modules;

internal class ServicesModule : NinjectModule
{
    public override void Load()
    {
        Bind<IVehicleService>().To<VehicleService>().InSingletonScope();
    }
}