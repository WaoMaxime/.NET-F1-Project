using Domain;

namespace DataAccessLayer;

public interface IRepository
{
    FastestLap ReadFastestLap(TimeSpan lapTime);
    IEnumerable<FastestLap> ReadAllFastestLaps();
    IEnumerable<FastestLap> ReadFastestLapsByCircuit(string circuit);
    void CreateFastestLap(FastestLap lap);
    F1Car ReadF1Car(int id);
    IEnumerable<F1Car> ReadAllF1Cars();
    IEnumerable<F1Car> ReadF1CarsByTeam(F1Team team);
    IEnumerable<Race> ReadAllRaces();
    Race ReadRace(int id);
    void CreateRace(Race race);
    IEnumerable<F1Car> ReadAllF1CarsWithTyresAndFastestLaps();
    IEnumerable<Race> ReadAllRacesWithFastestLapsAndCars();
    void AddCarTyre(CarTyre carTyre);
    void RemoveCarTyre(int carId, TyreType tyreType);
    IEnumerable<CarTyre> ReadCarTyresForCar(int carId);
    void CreateF1Car(F1Car car);
}