using Microsoft.EntityFrameworkCore;
using Mono.Contracts.Models;

namespace Mono.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){

    }

    public virtual DbSet<VehicleMake> VehicleMakes {get; set;}
    public virtual DbSet<VehicleModel> VehicleModels {get; set;}
}
