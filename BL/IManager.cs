using Domain;

namespace BusinessLayer
{
    public interface IManager
    {
        FastestLap GetFastestLap(int id);
        IEnumerable<FastestLap> GetAllFastestLaps();
        IEnumerable<FastestLap> GetFastestLapsByCircuit(string circuit);
        FastestLap AddFastestLap(string circuit, int airTemperature, int trackTemperature, TimeSpan lapTime, DateTime dateOfRecord, F1Car car); 
        F1Car GetF1Car(int id);
        IEnumerable<F1Car> GetAllF1Cars();
        IEnumerable<F1Car> GetF1CarsByTeam(F1Team team);
        F1Car AddF1Car(F1Team team, string chasis, int constructorsPosition, double driversPositions, DateTime manufactureDate, TyreType tyres, double? enginePower = null);
    }
}