using System.ComponentModel.DataAnnotations;
namespace Domain;
public enum F1Team
{
    Mercedes,
    RedBull,
    Ferrari,
    Mclaren,
    AstonMartin,
    Alpine,
    VsCashApp,
    Haas,
    KickSauber,
    Williams
}

public class F1Car : IValidatableObject
{
    [Key]
    public int Id { get; init; }

    [Required]
    public F1Team Team { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Chasis { get; set; }

    [Range(1, 10)]
    public int ConstructorsPosition { get; set; }

    [Range(0, 50)]
    public double DriversPositions { get; set; }
        
    public DateTime ManufactureDate { get; set; }

    [Required]
    public TyreType Tyres { get; set; }

    [Range(500, 1500)]
    public double? EnginePower { get; set; }

    public ICollection<FastestLap> FastestLaps { get; set; } = new List<FastestLap>();
       
    public ICollection<CarTyre> CarTyres { get; set; } = new List<CarTyre>();

    public F1Car(F1Team team, string chasis, int constructorsPosition, double driversPositions,
        DateTime manufactureDate, TyreType tyres, double? enginePower = null)
    {
        Team = team;
        Chasis = chasis;
        ConstructorsPosition = constructorsPosition;
        DriversPositions = driversPositions;
        ManufactureDate = manufactureDate;
        EnginePower = enginePower;
        Tyres = tyres;
    }

    public F1Car()
    {
        Team = F1Team.Mercedes;
        Chasis = "Default Chassis";
        ConstructorsPosition = 1;
        DriversPositions = 1.0;
        ManufactureDate = DateTime.Now.AddYears(-1);
        Tyres = TyreType.Medium;
        EnginePower = 1000; 
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