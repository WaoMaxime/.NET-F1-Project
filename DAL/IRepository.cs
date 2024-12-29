using Domain;

namespace DataAccessLayer;

public interface IRepository
{
    F1Car ReadF1Car(int id);
    F1Car ReadF1CarWithDetails(int id);
    CarTyre ReadTyreById(int id);
    Race ReadRace(int id);
    IEnumerable<FastestLap> ReadFastestLapsByTime(TimeSpan lapTime);
    IEnumerable<FastestLap> ReadAllFastestLaps();
    IEnumerable<FastestLap> ReadFastestLapsByCircuit(string circuit);
    IEnumerable<F1Car> ReadAllF1Cars();
    IEnumerable<F1Car> ReadF1CarsByTeam(F1Team team);
    IEnumerable<Race> ReadAllRaces();
    IEnumerable<F1Car> ReadAllF1CarsWithTyresAndFastestLaps();
    IEnumerable<Race> ReadAllRacesWithFastestLapsAndCars();
    IEnumerable<CarTyre> ReadCarTyresForCarById(int carId);
    void AddCarTyre(CarTyre carTyre);
    void RemoveCarTyre(int carId, TyreType tyreType);
    void CreateFastestLap(FastestLap lap);
    void CreateF1Car(F1Car car);
    void CreateRace(Race race);
}