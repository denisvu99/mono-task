using System.ComponentModel.DataAnnotations;

namespace Mono.Contracts.Models;

public class CreateVehicleModelVM {
    [Required]
    public string Name {get; set;}
    public int? ManufacturerId {get; set;}
    public ICollection<ViewBagManufacturer> ManufacturersList {get; set;}
}