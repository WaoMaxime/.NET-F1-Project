using Domain;
using Microsoft.AspNetCore.Identity;

namespace BusinessLayer
{
    public interface IManager
    {
        IEnumerable<FastestLap> GetFastestLapByTime(TimeSpan lapTime);
        IEnumerable<FastestLap> GetAllFastestLaps();
        IEnumerable<FastestLap> GetFastestLapsByCircuit(string circuit);
        IEnumerable<F1Car> GetAllF1CarsWithDetails();
        IEnumerable<Race> GetAllRacesWithDetails();
        IEnumerable<F1Car> GetF1CarsByTeam(F1Team team);
        IEnumerable<F1Car> GetAllF1Cars();
        IEnumerable<Race> GetAllRaces();
        IEnumerable<CarTyre> GetCarTyresForCarById(int carId);
        F1Car AddF1Car(F1Team team, string chasis, int constructorsPosition, double driversPositions, DateTime manufactureDate, TyreType tyres, double? enginePower = null);
        F1Car AddF1Car(F1Team team, string chasis, int constructorsPosition, double driversPositions, DateTime manufactureDate, TyreType tyres, IdentityUser user, double? enginePower = null);
        F1Car AddF1Car(F1Team team, string chasis, int constructorsPosition, double driversPositions, DateTime manufactureDate, TyreType tyres, IdentityUser user,string userId  , double? enginePower = null);
        void AddRace(Race race);
        void RemoveTyreFromCar(int carId, TyreType tyreType);
        F1Car ChangeHpF1Car(int carId, double newHp);
        CarTyre AddTyreToCar(int carId, TyreType tyreType, int tyrePressure, int operationalTemperature);
        FastestLap AddFastestLap(string circuit, int airTemperature, int trackTemperature, TimeSpan lapTime, DateTime dateOfRecord, F1Car car, Race race);
        F1Car GetF1Car(int id);
        F1Car GetF1CarWithDetails(int id);
        CarTyre GetTyreById(int id);
        Race GetRace(int id);
    }
}