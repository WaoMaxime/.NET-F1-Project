using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Race
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; } = null!;
    public DateTime Date { get; set; }
    public CarTyre Tyre { get; set; }

    public ICollection<FastestLap> FastestLaps { get; set; } = new List<FastestLap>();
}