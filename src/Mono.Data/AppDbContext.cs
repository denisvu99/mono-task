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
        VehicleMake toyota = new() {VehicleMakeId = 1, Name = "Toyota"};
        VehicleMake subaru = new() {VehicleMakeId = 2, Name = "Subaru"};
        VehicleMake nissan = new() {VehicleMakeId = 3, Name = "Nissan"};
        VehicleMake honda = new() {VehicleMakeId = 4, Name = "Honda"};

        VehicleModel t1 = new() {VehicleModelId = 1, Name="GR86", VehicleMakeId = 1};
        VehicleModel t2 = new() {VehicleModelId = 2, Name="Corolla", VehicleMakeId = 1};
        VehicleModel t3 = new() {VehicleModelId = 3, Name="Supra", VehicleMakeId = 2};
        VehicleModel s1 = new() {VehicleModelId = 4, Name="BRZ", VehicleMakeId = 2};
        VehicleModel s2 = new() {VehicleModelId = 5, Name="WRX", VehicleMakeId = 2};
        VehicleModel s3 = new() {VehicleModelId = 6, Name="Legacy", VehicleMakeId = 2};
        VehicleModel n1 = new() {VehicleModelId = 7, Name="GT-R", VehicleMakeId = 3};
        VehicleModel n2 = new() {VehicleModelId = 8, Name="370Z", VehicleMakeId = 3};
        VehicleModel n3 = new() {VehicleModelId = 9, Name="350Z", VehicleMakeId = 3};
        VehicleModel h1 = new() {VehicleModelId = 10, Name="NSX", VehicleMakeId = 4};
        VehicleModel h2 = new() {VehicleModelId = 11, Name="CRX", VehicleMakeId = 4};
        VehicleModel h3 = new() {VehicleModelId = 12, Name="Type R", VehicleMakeId = 4};

        modelBuilder.Entity<VehicleMake>().HasData(toyota,subaru,nissan,honda);
        modelBuilder.Entity<VehicleModel>().HasData(t1,t2,t3,s1,s2,s3,n1,n2,n3,h1,h2,h3);
    }
}
