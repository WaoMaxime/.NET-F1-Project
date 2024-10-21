namespace DataAccessLayer.EF;
using Domain;

public static class DataSeeder
{
    public static void Seed(F1CarDbContext context)
    {
        if (context.F1Cars.Any() || context.FastestLaps.Any())
        {
            return;  
        }
        
        var car1 = new F1Car(
            team: F1Team.RedBull,
            chasis: "RB19",
            constructorsPosition: 1,
            driversPositions: 1,
            manufactureDate: new DateTime(2023, 5, 12),
            tyres: TyreType.Soft,
            enginePower: 1000
        );

        var car2 = new F1Car(
            team: F1Team.Mercedes,
            chasis: "W14",
            constructorsPosition: 2,
            driversPositions: 2,
            manufactureDate: new DateTime(2023, 4, 8),
            tyres: TyreType.Medium,
            enginePower: 980
        );
        
        var lap1 = new FastestLap(
            circuit: "Monaco",
            airTemperature: 25,
            trackTemperature: 30,
            lapTime: new TimeSpan(0, 0, 1, 9,268),  
            dateOfRecord: new DateTime(2023, 5, 29),
            car: car1
        );

        var lap2 = new FastestLap(
            circuit: "Silverstone",
            airTemperature: 20,
            trackTemperature: 25,
            lapTime: new TimeSpan(0, 0, 1, 40,854),
            dateOfRecord: new DateTime(2023, 6, 11),
            car: car2
        );
        
        context.F1Cars.AddRange(car1, car2);
        context.FastestLaps.AddRange(lap1, lap2);
        
        context.SaveChanges();
        
        context.ChangeTracker.Clear();
    }
}
