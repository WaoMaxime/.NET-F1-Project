using System.ComponentModel.DataAnnotations;
using DataAccessLayer;
using Domain;
namespace BusinessLayer;
public class Manager : IManager
{
    private readonly IRepository _repository;

    public Manager(IRepository repository)
    {
        _repository = repository;
    }
    
    public IEnumerable<FastestLap> GetFastestLapByTime(TimeSpan lapTime)
    {
        return _repository.ReadFastestLapsByTime(lapTime);
    }

    public IEnumerable<FastestLap> GetAllFastestLaps() => _repository.ReadAllFastestLaps();

    public IEnumerable<FastestLap> GetFastestLapsByCircuit(string circuit) => _repository.ReadFastestLapsByCircuit(circuit);

    public IEnumerable<F1Car> GetAllF1CarsWithDetails()
    {
        return _repository.ReadAllF1CarsWithTyresAndFastestLaps();
    }

    public IEnumerable<Race> GetAllRacesWithDetails()
    {
        return _repository.ReadAllRacesWithFastestLapsAndCars();
    }
    
    public IEnumerable<F1Car> GetF1CarsByTeam(F1Team team) => _repository.ReadF1CarsByTeam(team);
        
    public IEnumerable<F1Car> GetAllF1Cars() => _repository.ReadAllF1Cars();
        
    public IEnumerable<Race> GetAllRaces() => _repository.ReadAllRaces();
    
    public IEnumerable<CarTyre> GetCarTyresForCarById(int carId)
    {
        return _repository.ReadCarTyresForCarById(carId);
    }

    public CarTyre AddTyreToCar(int carId, TyreType tyreType, int tyrePressure, int operationalTemperature)
    {
        var car = _repository.ReadF1Car(carId);
        if (car == null) throw new Exception("Car not found!");

        var newCarTyre = new CarTyre
        {
            Car = car,
            Tyre = tyreType,
            TyrePressure = tyrePressure,
            OperationalTemperature = operationalTemperature
        };
        _repository.AddCarTyre(newCarTyre);
        return newCarTyre;
    } 
        
    public void AddF1Car(F1Team team, string chasis, int constructorsPosition, double driversPositions, DateTime manufactureDate, TyreType tyres, double? enginePower = null)
    {
        var newCar = new F1Car(team, chasis, constructorsPosition, driversPositions, manufactureDate, tyres, enginePower);
        ValidateModel(newCar);
        _repository.CreateF1Car(newCar);
    }
        
    public void AddRace(Race race)
    {
        _repository.CreateRace(race);
    }
        
    public void RemoveTyreFromCar(int carId, TyreType tyreType)
    {
        _repository.RemoveCarTyre(carId, tyreType);
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
        _repository.CreateFastestLap(newLap);
        return newLap;
    }
        
    public F1Car GetF1Car(int id) => _repository.ReadF1Car(id);
    public F1Car GetF1CarWithDetails(int id)
    {
        return _repository.ReadF1CarWithDetails(id);
    }

    public CarTyre GetTyreById(int id)
    {
        return _repository.ReadTyreById(id);
    }

    public Race GetRace(int id) => _repository.ReadRace(id);
        
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
