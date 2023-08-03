using System.ComponentModel.DataAnnotations;

namespace Mono.Contracts.Models;

public class ManufacturersVM{
    public int VehicleMakeId {get; set;}
    [Required]
    public string ManufacturerName {get; set;}
    public int Count {get; set;}
}