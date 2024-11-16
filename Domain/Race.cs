using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Race
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Race name is required.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Race name must be between 3 and 100 characters.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Race date is required.")]
    public DateTime Date { get; set; }
    public ICollection<CarTyre> CarTyres { get; set; } = new List<CarTyre>();

    public ICollection<FastestLap> FastestLaps { get; set; } = new List<FastestLap>();
}