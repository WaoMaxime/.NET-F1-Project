using Domain;

namespace DataAccessLayer
{
    public class InMemoryRepository : IRepository
    {
        public static List<FastestLap> FastestLaps;
        public static List<F1Car> F1Cars;
        
        public static void Seed()
        {
            FastestLaps.Clear();
            F1Cars.Clear();
            
            F1Cars.AddRange(new List<F1Car>
            {
                new F1Car(F1Team.RedBull, "RB19", 1, 1.3, new DateTime(2023, 2, 10), TyreType.Soft, 1050) { Id = 1 },
                new F1Car(F1Team.Mercedes, "W14", 2, 2.4, new DateTime(2023, 3, 15), TyreType.Medium, 1020) { Id = 2 },
                new F1Car(F1Team.Ferrari, "SF23", 3, 3.7, new DateTime(2023, 4, 20), TyreType.Hard, 1000) { Id = 3 }
            });
            
            FastestLaps.AddRange(new List<FastestLap>
            {
                new FastestLap("Monza", 25, 35, new TimeSpan(0, 1, 19), DateTime.Now, F1Cars[0]) { Id = 1 },
                new FastestLap("Silverstone", 20, 30, new TimeSpan(0, 1, 27), DateTime.Now, F1Cars[1]) { Id = 2 },
                new FastestLap("Spa", 18, 28, new TimeSpan(0, 1, 42), DateTime.Now, F1Cars[2]) { Id = 3 }
            });
        }
        
        public FastestLap ReadFastestLap(TimeSpan lapTime)
        {
            return FastestLaps.FirstOrDefault(l => l.LapTime == lapTime );
        }

        public IEnumerable<FastestLap> ReadAllFastestLaps()
        {
            return FastestLaps;
        }
        
        public IEnumerable<FastestLap> ReadFastestLapsByPartialTime(TimeSpan partialLapTime)
        {
            return FastestLaps.Where(lap => lap.LapTime.Minutes == partialLapTime.Minutes &&
                                                     lap.LapTime.Seconds / 10 == partialLapTime.Seconds / 10);
        }

        public IEnumerable<FastestLap> ReadFastestLapsByCircuit(string circuit)
        {
            return FastestLaps.Where(lap => lap.Circuit == circuit);
        }

        public void CreateFastestLap(FastestLap lap)
        {
            lap.Id = FastestLaps.Count + 1; 
            FastestLaps.Add(lap);
        }
        
        public F1Car ReadF1Car(int id)
        {
            return F1Cars.FirstOrDefault(car => car.Id == id);
        }

        public IEnumerable<F1Car> ReadAllF1Cars()
        {
            return F1Cars;
        }

        public IEnumerable<F1Car> ReadF1CarsByTeam(F1Team team)
        {
            return F1Cars.Where(car => car.Team == team);
        }

        public void CreateF1Car(F1Car car)
        {
            car.Id = F1Cars.Count + 1; 
            F1Cars.Add(car);
        }
    }
}
