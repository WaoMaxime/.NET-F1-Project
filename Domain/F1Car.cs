using Sprint1;

namespace Domain
{
    public enum F1Team
    {
        Mercedes,
        RedBull,
        Ferrari,
        McLaren,
        AstonMartin,
        Alpine,
        VsCashApp,
        Haas,
        KickSauber,
        Williams
    }
    public class F1Car
    {
        public F1Team Team { get; set; }
        public string Chasis { get; set; }
        public int ConstructorsPosition { get; set; }
        public double DriversPositions { get; set; }
        public DateTime ManufactureDate { get; set; }  
        public TyreType Tyres { get; set; } 
        public double? EnginePower { get; set; } 

        public F1Car(F1Team team, string chasis, int constructorsPosition, double driversPositions, DateTime manufactureDate,  TyreType tyres, double? enginePower = null)
        {
            Team = team;
            Chasis = chasis;
            ConstructorsPosition = constructorsPosition;
            DriversPositions = driversPositions;
            ManufactureDate = manufactureDate;
            EnginePower = enginePower;
            Tyres = tyres;
        }

        public override string ToString()
        {
            return string.Format("Team {0} with chassis {1}, Constructors Position: {2}, Drivers Position: {3}, Manufactured on: {4:dd-MM-yyyy}, Engine Power: {5} HP",
                Team, Chasis, ConstructorsPosition, DriversPositions, ManufactureDate, EnginePower.HasValue ? EnginePower.ToString() : "N/A") + "\n--------------------------------------------------------------------------------------------------------------------------------------";
        }
    }
}