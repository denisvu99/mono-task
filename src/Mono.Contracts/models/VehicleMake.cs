using System.ComponentModel.DataAnnotations;

namespace Mono.Contracts.Models;

public class VehicleMake {
    [Key]
    public int VehicleMakeId {get; set;}
    [Required]
    public string Name {get; set;}
    public ICollection<VehicleModel> VehicleModels {get; set;} = new List<VehicleModel>();
}