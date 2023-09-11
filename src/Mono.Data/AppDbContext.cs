using Microsoft.EntityFrameworkCore;
using Mono.Contracts.Models;

namespace Mono.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){
        
    }

    public virtual DbSet<VehicleMake> VehicleMakes {get; set;}
    public virtual DbSet<VehicleModel> VehicleModels {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        VehicleMake toyota = new() {VehicleMakeId = 1, ManufacturerName = "Toyota"};
        VehicleMake subaru = new() {VehicleMakeId = 2, ManufacturerName = "Subaru"};
        VehicleMake nissan = new() {VehicleMakeId = 3, ManufacturerName = "Nissan"};
        VehicleMake honda = new() {VehicleMakeId = 4, ManufacturerName = "Honda"};

        VehicleModel t1 = new() {VehicleModelId = 1, ModelName="GR86", VehicleMakeId = 1};
        VehicleModel t2 = new() {VehicleModelId = 2, ModelName="Corolla", VehicleMakeId = 1};
        VehicleModel t3 = new() {VehicleModelId = 3, ModelName="Supra", VehicleMakeId = 1};
        VehicleModel t4 = new() {VehicleModelId = 13, ModelName="Camry", VehicleMakeId = 1};
        VehicleModel t5 = new() {VehicleModelId = 14, ModelName="Prius", VehicleMakeId = 1};
        VehicleModel t6 = new() {VehicleModelId = 15, ModelName="Yaris", VehicleMakeId = 1};
        VehicleModel t7 = new() {VehicleModelId = 16, ModelName="Crown", VehicleMakeId = 1};
        VehicleModel s1 = new() {VehicleModelId = 4, ModelName="BRZ", VehicleMakeId = 2};
        VehicleModel s2 = new() {VehicleModelId = 5, ModelName="WRX", VehicleMakeId = 2};
        VehicleModel s3 = new() {VehicleModelId = 6, ModelName="Legacy", VehicleMakeId = 2};
        VehicleModel n1 = new() {VehicleModelId = 7, ModelName="GT-R", VehicleMakeId = 3};
        VehicleModel n2 = new() {VehicleModelId = 8, ModelName="370Z", VehicleMakeId = 3};
        VehicleModel n3 = new() {VehicleModelId = 9, ModelName="350Z", VehicleMakeId = 3};
        VehicleModel h1 = new() {VehicleModelId = 10, ModelName="NSX", VehicleMakeId = 4};
        VehicleModel h2 = new() {VehicleModelId = 11, ModelName="CRX", VehicleMakeId = 4};
        VehicleModel h3 = new() {VehicleModelId = 12, ModelName="Type R", VehicleMakeId = 4};
        VehicleModel h4 = new() {VehicleModelId = 17, ModelName="Civic", VehicleMakeId = 4};
        VehicleModel h5 = new() {VehicleModelId = 18, ModelName="Accord", VehicleMakeId = 4};

        modelBuilder.Entity<VehicleMake>().HasData(toyota,subaru,nissan,honda);
        modelBuilder.Entity<VehicleModel>().HasData(t1,t2,t3,t4,t5,t6,t7,s1,s2,s3,n1,n2,n3,h1,h2,h3,h4,h5);
    }
}
