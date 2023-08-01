using Ninject.Modules;
using Mono.Data;
using Mono.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;

public class DataModule : NinjectModule
{
    private string _connectionString = "Host=localhost;Database=mono;Username=user;Password=123";
    public override void Load()
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(_connectionString);
        Bind<AppDbContext>().ToSelf().InSingletonScope().WithConstructorArgument("options",optionsBuilder.Options);
        Bind<IVehicleMakeRepository>().To<VehicleMakeRepository>();
        Bind<IVehicleModelRepository>().To<VehicleModelRepository>();
    }
}