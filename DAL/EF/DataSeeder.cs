namespace DataAccessLayer.EF;
using Domain;

public class DataSeeder
{
    public void Seed(F1CarDbContext context)
    {
        if (context.F1Cars.Any() || context.FastestLaps.Any() || context.Races.Any())
        {
            return;  
        }
        
        var races = new List<Race>
        {
            new() { Name = "Monaco GP", Date = new DateTime(2023, 5, 28) },
            new() { Name = "Silverstone GP", Date = new DateTime(2023, 7, 9) },
            new() { Name = "Belgian GP", Date = new DateTime(2023, 8, 27) },
            new() { Name = "Spanish GP", Date = new DateTime(2023, 6, 4) },
            new() { Name = "Italian GP", Date = new DateTime(2023, 9, 3) }
        };

        context.AddRange(races);
        context.SaveChanges();
        
        var cars = new List<F1Car>
        {
            new(F1Team.RedBull, "RB19", 1, 1, new DateTime(2023, 2, 10), TyreType.Soft, 1050),
            new(F1Team.Mercedes, "W14", 2, 2, new DateTime(2023, 3, 15), TyreType.Medium, 1020),
            new(F1Team.Ferrari, "SF23", 3, 3, new DateTime(2023, 4, 20), TyreType.Hard, 1000),
            new(F1Team.Mclaren, "MCL60", 4, 5, new DateTime(2023, 5, 10), TyreType.Medium, 950),
            new(F1Team.AstonMartin, "AMR23", 5, 6, new DateTime(2023, 6, 18), TyreType.Hard, 980)
        };

        context.AddRange(cars);
        context.SaveChanges();
        
        var fastestLaps = new List<FastestLap>
        {
            new("Monaco", 25, 35, new TimeSpan(0, 1, 19), new DateTime(2023, 5, 28), cars[0], races[0]),
            new("Silverstone", 20, 30, new TimeSpan(0, 1, 27), new DateTime(2023, 7, 9), cars[1], races[1]),
            new("Spa", 18, 28, new TimeSpan(0, 1, 42), new DateTime(2023, 8, 27), cars[2], races[2]),
            new("Barcelona", 22, 32, new TimeSpan(0, 1, 35), new DateTime(2023, 6, 4), cars[3], races[3]),
            new("Monza", 24, 34, new TimeSpan(0, 1, 20), new DateTime(2023, 9, 3), cars[4], races[4])
        };

        context.AddRange(fastestLaps);
        context.SaveChanges();
        
        var carTyres = new List<CarTyre>
        {
            new() { Car = cars[0], Tyre = TyreType.Soft, TyrePressure = 20, OperationalTemperature = 90, RaceId = 1 },
            new() { Car = cars[1], Tyre = TyreType.Medium, TyrePressure = 21, OperationalTemperature = 95, RaceId = 2 },
            new() { Car = cars[2], Tyre = TyreType.Hard, TyrePressure = 22, OperationalTemperature = 100, RaceId = 3 },
            new() { Car = cars[3], Tyre = TyreType.Medium, TyrePressure = 19, OperationalTemperature = 88, RaceId = 4 },
            new() { Car = cars[4], Tyre = TyreType.Hard, TyrePressure = 23, OperationalTemperature = 105, RaceId = 5 }
        };

        context.AddRange(carTyres);
        context.SaveChanges();

        context.ChangeTracker.Clear();
    }
}
