using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mono.Contracts.Models;

public class VehicleModel{
    [Key]
    public int VehicleModelId {get; set;}
    [Required]
    public string Name {get; set;}
    public int? VehicleMakeId {get; set;}
    [ForeignKey("VehicleMakeId")]
    public VehicleMake? VehicleMake {get; set;}
}