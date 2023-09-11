using Ninject.Modules;
using Mono.Data;
using Mono.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using Ninject;

internal class DataModule : NinjectModule
{
    private string _connectionString = "Host=localhost;Database=mono;Username=user;Password=123";
    public override void Load()
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(_connectionString);
        Bind<AppDbContext>().ToSelf().InSingletonScope().WithConstructorArgument("options",optionsBuilder.Options);
        Bind<IVehicleMakeRepository>().To<VehicleMakeRepository>().InThreadScope().WithConstructorArgument("db", ctx => ctx.Kernel.Get<AppDbContext>());
        Bind<IVehicleModelRepository>().To<VehicleModelRepository>().InThreadScope().WithConstructorArgument("db", ctx => ctx.Kernel.Get<AppDbContext>());
    }
}