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
    void CreateF1Car(F1Car car);
}