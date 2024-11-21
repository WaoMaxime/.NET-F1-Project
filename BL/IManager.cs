using Domain;

namespace BusinessLayer
{
    public interface IManager
    {
        IEnumerable<FastestLap> GetFastestLapByTime(TimeSpan lapTime);
        IEnumerable<FastestLap> GetAllFastestLaps();
        IEnumerable<FastestLap> GetFastestLapsByCircuit(string circuit);
        IEnumerable<F1Car> GetAllF1CarsWithDetails();
        IEnumerable<Race> GetAllRacesWithDetails();
        IEnumerable<CarTyre> GetCarTyresForCarById(int carId);
        IEnumerable<F1Car> GetF1CarsByTeam(F1Team team);
        IEnumerable<F1Car> GetAllF1Cars();
        IEnumerable<Race> GetAllRaces();
        void AddTyreToCar(int carId, TyreType tyreType, int tyrePressure, int operationalTemperature);
        void AddF1Car(F1Team team, string chasis, int constructorsPosition, double driversPositions, DateTime manufactureDate, TyreType tyres, double? enginePower = null);
        void AddRace(Race race);
        void RemoveTyreFromCar(int carId, TyreType tyreType);
        FastestLap AddFastestLap(string circuit, int airTemperature, int trackTemperature, TimeSpan lapTime, DateTime dateOfRecord, F1Car car, Race race);
        F1Car GetF1Car(int id);
        Race GetRace(int id);
    }
}