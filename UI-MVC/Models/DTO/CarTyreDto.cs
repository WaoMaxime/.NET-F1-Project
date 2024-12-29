using System.ComponentModel.DataAnnotations;
using Domain;

namespace UI.DTO;

public class CarTyreDto
{
    [Required]
    public int CarId { get; set; }

    [Required]
    public TyreType Tyre { get; set; }

    [Range(10, 50)]
    public int TyrePressure { get; set; }

    [Range(50, 150)]
    public int OperationalTemperature { get; set; }
}