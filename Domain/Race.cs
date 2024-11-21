using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Race
{
    [Key]
    public int Id { get; init; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; init; } = null!;
    public DateTime Date { get; init; }
    public CarTyre Tyre { get; init; }

    public ICollection<FastestLap> FastestLaps { get; set; } = new List<FastestLap>();
}