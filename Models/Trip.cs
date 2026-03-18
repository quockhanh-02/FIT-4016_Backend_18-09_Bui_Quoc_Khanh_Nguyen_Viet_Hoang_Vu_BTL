using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FleetManagement.Models;

public class Trip
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string StartLocation { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string EndLocation { get; set; } = string.Empty;

    // =========================
    // Foreign Key: Vehicle
    // =========================
    [ForeignKey("Vehicle")]
    public int VehicleId { get; set; }

    public Vehicle? Vehicle { get; set; }

    // =========================
    // Foreign Key: Driver
    // =========================
    [ForeignKey("Driver")]
    public int DriverId { get; set; }

    public Driver? Driver { get; set; }
}