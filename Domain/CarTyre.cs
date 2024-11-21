using System.ComponentModel.DataAnnotations;

namespace Domain;

public class CarTyre
{
    [Required]
    public int CarId { get; init; }

    public F1Car Car { get; init; }

    [Required]
    public TyreType Tyre { get; init; }

    [Range(10, 50)]
    public int TyrePressure { get; init; }

    [Range(50, 150)]
    public int OperationalTemperature { get; init; }

    public int? RaceId { get; init; } 

    public Race Race { get; init; }
}
