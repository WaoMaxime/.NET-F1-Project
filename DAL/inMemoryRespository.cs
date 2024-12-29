using Domain;
namespace DataAccessLayer;

public class InMemoryRepository : IRepository
{
    private static readonly List<FastestLap> FastestLaps = [];
    private static readonly List<F1Car> F1Cars = [];
    private static readonly List<Race> Races = [];
    private static readonly List<CarTyre> CarTyres = [];

    public static void Seed()
    {
        FastestLaps.Clear();
        F1Cars.Clear();
        Races.Clear();
        CarTyres.Clear();

        Races.AddRange(new List<Race>
        {
            new() { Id = 1, Name = "Monaco GP", Date = new DateTime(2023, 5, 28) },
            new() { Id = 2, Name = "Silverstone GP", Date = new DateTime(2023, 7, 9) },
            new() { Id = 3, Name = "Belgian GP", Date = new DateTime(2023, 8, 27) },
            new() { Id = 4, Name = "Spanish GP", Date = new DateTime(2023, 6, 4) },
            new() { Id = 5, Name = "Italian GP", Date = new DateTime(2023, 9, 3) }
        });

        F1Cars.AddRange(new List<F1Car>
        {
            new(F1Team.RedBull, "RB19", 1, 1.3, new DateTime(2023, 2, 10), TyreType.Soft, 1050) { Id = 1 },
            new(F1Team.Mercedes, "W14", 2, 2.4, new DateTime(2023, 3, 15), TyreType.Medium, 1020) { Id = 2 },
            new(F1Team.Ferrari, "SF23", 3, 3.7, new DateTime(2023, 4, 20), TyreType.Hard, 1000) { Id = 3 },
            new(F1Team.Mclaren, "MCL60", 4, 5.1, new DateTime(2023, 5, 10), TyreType.Medium, 950) { Id = 4 },
            new(F1Team.AstonMartin, "AMR23", 5, 6.2, new DateTime(2023, 6, 18), TyreType.Hard, 980) { Id = 5 }
        });

        FastestLaps.AddRange(new List<FastestLap>
        {
            new("Monaco", 25, 35, new TimeSpan(0, 1, 19), new DateTime(2023, 5, 28), F1Cars[0], Races[0]) { Id = 1 },
            new("Silverstone", 20, 30, new TimeSpan(0, 1, 27), new DateTime(2023, 7, 9), F1Cars[1], Races[1]) { Id = 2 },
            new("Spa", 18, 28, new TimeSpan(0, 1, 42), new DateTime(2023, 8, 27), F1Cars[2], Races[2]) { Id = 3 },
            new("Barcelona", 22, 32, new TimeSpan(0, 1, 35), new DateTime(2023, 6, 4), F1Cars[3], Races[3]) { Id = 4 },
            new("Monza", 24, 34, new TimeSpan(0, 1, 20), new DateTime(2023, 9, 3), F1Cars[4], Races[4]) { Id = 5 }
        });

        CarTyres.AddRange(new List<CarTyre>
        {
            new() { CarId = 1, Tyre = TyreType.Soft, TyrePressure = 20, OperationalTemperature = 90, RaceId = 1 },
            new() { CarId = 2, Tyre = TyreType.Medium, TyrePressure = 21, OperationalTemperature = 95, RaceId = 2 },
            new() { CarId = 3, Tyre = TyreType.Hard, TyrePressure = 22, OperationalTemperature = 100, RaceId = 3 },
            new() { CarId = 4, Tyre = TyreType.Medium, TyrePressure = 19, OperationalTemperature = 88, RaceId = 4 },
            new() { CarId = 5, Tyre = TyreType.Hard, TyrePressure = 23, OperationalTemperature = 105, RaceId = 5 }
        });
            
        foreach (var carTyre in CarTyres)
        {
            carTyre.Car.CarTyres.Add(carTyre);
        }
    }
        
    public F1Car ReadF1Car(int id) => F1Cars.FirstOrDefault(car => car.Id == id);
    public F1Car ReadF1CarWithDetails(int id) => F1Cars.FirstOrDefault(car => car.Id == id);
    public Race ReadRace(int id) => Races.FirstOrDefault(race => race.Id == id);
        
    public IEnumerable<FastestLap> ReadFastestLapsByTime(TimeSpan lapTime) => FastestLaps.Where(lap => lap.LapTime == lapTime);
        
    public IEnumerable<FastestLap> ReadAllFastestLaps() => FastestLaps;
        
    public IEnumerable<FastestLap> ReadFastestLapsByCircuit(string circuit) => FastestLaps.Where(lap => lap.Circuit == circuit);
        
    public IEnumerable<F1Car> ReadAllF1Cars() => F1Cars;
        
    public IEnumerable<F1Car> ReadF1CarsByTeam(F1Team team) => F1Cars.Where(car => car.Team == team);
        
    public IEnumerable<Race> ReadAllRaces() => Races;
        
    public IEnumerable<F1Car> ReadAllF1CarsWithTyresAndFastestLaps()
    {
        return F1Cars.Select(car =>
        {
            car.CarTyres = CarTyres.Where(ct => ct.Car.Id == car.Id).ToList();
            car.FastestLaps = FastestLaps.Where(fl => fl.Car.Id == car.Id).ToList();
            return car;
        }).ToList();
    }

    public IEnumerable<Race> ReadAllRacesWithFastestLapsAndCars()
    {
        return Races.Select(race =>
        {
            race.FastestLaps = FastestLaps.Where(fl => fl.Race.Id == race.Id).ToList();
            foreach (var lap in race.FastestLaps)
            {
                lap.Car = F1Cars.FirstOrDefault(car => car.Id == lap.Car.Id);
            }
            return race;
        }).ToList();
    }
    
    public IEnumerable<CarTyre> ReadCarTyresForCarById(int carId)
    {
        return CarTyres
            .Where(ct => ct.CarId == carId)
            .ToList();
    }
    
    public CarTyre ReadTyreById(int id)
    {
        return CarTyres.FirstOrDefault(ct => ct.CarId == id);
    }
    
    public void AddCarTyre(CarTyre carTyre)
    {
        carTyre.Car.CarTyres.Add(carTyre);
        CarTyres.Add(carTyre);
    }

    public void RemoveCarTyre(int carId, TyreType tyreType)
    {
        var carTyre = CarTyres.FirstOrDefault(ct => ct.Car.Id == carId && ct.Tyre == tyreType);
        if (carTyre == null) return;
        carTyre.Car.CarTyres.Remove(carTyre);
        CarTyres.Remove(carTyre);
    }
        
    public void CreateFastestLap(FastestLap lap) => FastestLaps.Add(lap);
        
        
    public void CreateF1Car(F1Car car) => F1Cars.Add(car);
        
        
    public void CreateRace(Race race) => Races.Add(race);
}