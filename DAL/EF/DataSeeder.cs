namespace DataAccessLayer.EF;
using Domain;

public static class DataSeeder
{
    public static void Seed(F1CarDbContext context)
    {
        if (context.F1Cars.Any() || context.FastestLaps.Any() || context.Races.Any())
        {
            return;  
        }
        
        var races = new List<Race>
        {
            new Race { Name = "Monaco GP", Date = new DateTime(2023, 5, 28) },
            new Race { Name = "Silverstone GP", Date = new DateTime(2023, 7, 9) },
            new Race { Name = "Belgian GP", Date = new DateTime(2023, 8, 27) },
            new Race { Name = "Spanish GP", Date = new DateTime(2023, 6, 4) },
            new Race { Name = "Italian GP", Date = new DateTime(2023, 9, 3) }
        };

        context.AddRange(races);
        context.SaveChanges();
        
        var cars = new List<F1Car>
        {
            new F1Car(F1Team.RedBull, "RB19", 1, 1, new DateTime(2023, 2, 10), TyreType.Soft, 1050),
            new F1Car(F1Team.Mercedes, "W14", 2, 2, new DateTime(2023, 3, 15), TyreType.Medium, 1020),
            new F1Car(F1Team.Ferrari, "SF23", 3, 3, new DateTime(2023, 4, 20), TyreType.Hard, 1000),
            new F1Car(F1Team.Mclaren, "MCL60", 4, 5, new DateTime(2023, 5, 10), TyreType.Medium, 950),
            new F1Car(F1Team.AstonMartin, "AMR23", 5, 6, new DateTime(2023, 6, 18), TyreType.Hard, 980)
        };

        context.AddRange(cars);
        context.SaveChanges();
        
        var fastestLaps = new List<FastestLap>
        {
            new FastestLap("Monaco", 25, 35, new TimeSpan(0, 1, 19), new DateTime(2023, 5, 28), cars[0], races[0]),
            new FastestLap("Silverstone", 20, 30, new TimeSpan(0, 1, 27), new DateTime(2023, 7, 9), cars[1], races[1]),
            new FastestLap("Spa", 18, 28, new TimeSpan(0, 1, 42), new DateTime(2023, 8, 27), cars[2], races[2]),
            new FastestLap("Barcelona", 22, 32, new TimeSpan(0, 1, 35), new DateTime(2023, 6, 4), cars[3], races[3]),
            new FastestLap("Monza", 24, 34, new TimeSpan(0, 1, 20), new DateTime(2023, 9, 3), cars[4], races[4])
        };

        context.AddRange(fastestLaps);
        context.SaveChanges();
        
        var carTyres = new List<CarTyre>
        {
            new CarTyre { Car = cars[0], Tyre = TyreType.Soft, TyrePressure = 20, OperationalTemperature = 90, RaceId = 1 },
            new CarTyre { Car = cars[1], Tyre = TyreType.Medium, TyrePressure = 21, OperationalTemperature = 95, RaceId = 2 },
            new CarTyre { Car = cars[2], Tyre = TyreType.Hard, TyrePressure = 22, OperationalTemperature = 100, RaceId = 3 },
            new CarTyre { Car = cars[3], Tyre = TyreType.Medium, TyrePressure = 19, OperationalTemperature = 88, RaceId = 4 },
            new CarTyre { Car = cars[4], Tyre = TyreType.Hard, TyrePressure = 23, OperationalTemperature = 105, RaceId = 5 }
        };

        context.AddRange(carTyres);
        context.SaveChanges();

        context.ChangeTracker.Clear();
    }
}
