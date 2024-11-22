using System.ComponentModel.DataAnnotations;
using DataAccessLayer;
using Domain;
namespace BusinessLayer;
public class Manager(IRepository repository) : IManager
{
    public IEnumerable<FastestLap> GetFastestLapByTime(TimeSpan lapTime)
    {
        return repository.ReadFastestLapsByTime(lapTime);
    }

    public IEnumerable<FastestLap> GetAllFastestLaps() => repository.ReadAllFastestLaps();

    public IEnumerable<FastestLap> GetFastestLapsByCircuit(string circuit) => repository.ReadFastestLapsByCircuit(circuit);

    public IEnumerable<F1Car> GetAllF1CarsWithDetails()
    {
        return repository.ReadAllF1CarsWithTyresAndFastestLaps();
    }

    public IEnumerable<Race> GetAllRacesWithDetails()
    {
        return repository.ReadAllRacesWithFastestLapsAndCars();
    }
        
    public IEnumerable<CarTyre> GetCarTyresForCarById(int carId)
    {
        return repository.ReadCarTyresForCarById(carId);
    }
        
    public IEnumerable<F1Car> GetF1CarsByTeam(F1Team team) => repository.ReadF1CarsByTeam(team);
        
    public IEnumerable<F1Car> GetAllF1Cars() => repository.ReadAllF1Cars();
        
    public IEnumerable<Race> GetAllRaces() => repository.ReadAllRaces();

    public void AddTyreToCar(int carId, TyreType tyreType, int tyrePressure, int operationalTemperature)
    {
        var car = repository.ReadF1Car(carId);
        if (car == null) throw new Exception("Car not found!");

        var newCarTyre = new CarTyre
        {
            Car = car,
            Tyre = tyreType,
            TyrePressure = tyrePressure,
            OperationalTemperature = operationalTemperature
        };

        repository.AddCarTyre(newCarTyre);
    } 
        
    public void AddF1Car(F1Team team, string chasis, int constructorsPosition, double driversPositions, DateTime manufactureDate, TyreType tyres, double? enginePower = null)
    {
        var newCar = new F1Car(team, chasis, constructorsPosition, driversPositions, manufactureDate, tyres, enginePower);
        ValidateModel(newCar);
        repository.CreateF1Car(newCar);
    }
        
    public void AddRace(Race race)
    {
        repository.CreateRace(race);
    }
        
    public void RemoveTyreFromCar(int carId, TyreType tyreType)
    {
        repository.RemoveCarTyre(carId, tyreType);
    }
        
    public FastestLap AddFastestLap(
        string circuit,
        int airTemperature,
        int trackTemperature,
        TimeSpan lapTime,
        DateTime dateOfRecord,
        F1Car car,
        Race race)
    {
        if (race == null)
            throw new ArgumentNullException(nameof(race), "A race must be provided for the fastest lap.");

        var newLap = new FastestLap(circuit, airTemperature, trackTemperature, lapTime, dateOfRecord, car, race);
        ValidateModel(newLap);
        repository.CreateFastestLap(newLap);
        return newLap;
    }
        
    public F1Car GetF1Car(int id) => repository.ReadF1Car(id);
        
    public Race GetRace(int id) => repository.ReadRace(id);
        
    private static void ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(model);
        if (!Validator.TryValidateObject(model, context, validationResults, true))
        {
            throw new ValidationException("Validation failed for the following properties: " +
                                          string.Join(", ", validationResults));
        }
    }
}