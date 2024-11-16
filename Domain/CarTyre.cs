using System.ComponentModel.DataAnnotations;

namespace Domain;

public class CarTyre
{
    [Required]
    public int CarId { get; set; }

    public F1Car Car { get; set; } = null!;

    [Required]
    public TyreType Tyre { get; set; }

    [Range(10, 50, ErrorMessage = "Tyre pressure must be between 10 and 50 PSI.")]
    public int TyrePressure { get; set; }

    [Range(50, 150, ErrorMessage = "Operational temperature must be between 50 and 150 °C.")]
    public int OperationalTemperature { get; set; }

    public int? RaceId { get; set; } 

    public Race? Race { get; set; }
}
