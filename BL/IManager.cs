using Domain;

namespace BusinessLayer
{
    public interface IManager
    {
        FastestLap GetFastestLapByTime(TimeSpan lapTime);
        IEnumerable<FastestLap> GetAllFastestLaps();
        IEnumerable<FastestLap> GetFastestLapsByCircuit(string circuit);
        IEnumerable<F1Car> GetAllF1CarsWithDetails();
        IEnumerable<Race> GetAllRacesWithDetails();
        void AddTyreToCar(int carId, TyreType tyreType, int tyrePressure, int operationalTemperature);
        void RemoveTyreFromCar(int carId, TyreType tyreType);
        IEnumerable<CarTyre> GetCarTyresForCar(int carId);
        public FastestLap AddFastestLap(string circuit, int airTemperature, int trackTemperature, TimeSpan lapTime, DateTime dateOfRecord, F1Car car, Race race);
        F1Car GetF1Car(int id);
        IEnumerable<Race> GetAllRaces();
        Race GetRace(int id);
        void AddRace(Race race);
        IEnumerable<F1Car> GetAllF1Cars();
        IEnumerable<F1Car> GetF1CarsByTeam(F1Team team);
        F1Car AddF1Car(F1Team team, string chasis, int constructorsPosition, double driversPositions, DateTime manufactureDate, TyreType tyres, double? enginePower = null);
    }
}