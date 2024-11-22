using System.ComponentModel.DataAnnotations;
namespace Domain;
public enum F1Team
{
    Mercedes,
    RedBull,
    Ferrari,
    Mclaren,
    AstonMartin
}

public class F1Car(
    F1Team team,
    string chasis,
    int constructorsPosition,
    double driversPositions,
    DateTime manufactureDate,
    TyreType tyres,
    double? enginePower = null)
    : IValidatableObject
{
    [Key]
    public int Id { get; init; }

    [Required]
    public F1Team Team { get; set; } = team;

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Chasis { get; set; } = chasis;

    [Range(1, 10)]
    public int ConstructorsPosition { get; set; } = constructorsPosition;

    [Range(0, 50)]
    public double DriversPositions { get; set; } = driversPositions;

    public DateTime ManufactureDate { get; set; } = manufactureDate;

    [Required]
    public TyreType Tyres { get; set; } = tyres;

    [Range(500, 1500)]
    public double? EnginePower { get; set; } = enginePower;

    public ICollection<FastestLap> FastestLaps { get; set; } = new List<FastestLap>();
       
    public ICollection<CarTyre> CarTyres { get; set; } = new List<CarTyre>();

    public F1Car() : this(F1Team.Mercedes, "Default Chassis", 1, 1.0, DateTime.Now.AddYears(-1), TyreType.Medium, 1000)
    {
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EnginePower is <= 0)
        {
            yield return new ValidationResult("Engine Power must be a positive value.", [nameof(EnginePower)]);
        }

        if (ManufactureDate > DateTime.Now)
        {
            yield return new ValidationResult("Manufacture date cannot be in the future.", [nameof(ManufactureDate)]);
        }
    }
}