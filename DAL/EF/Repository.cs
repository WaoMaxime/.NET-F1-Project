using Domain;
using Microsoft.EntityFrameworkCore;


namespace DataAccessLayer.EF;

public class Repository : IRepository
{   
    private readonly F1CarDbContext _context;
    
    public Repository(F1CarDbContext context)
    {
        _context = context;
    }
    
    public FastestLap ReadFastestLap(TimeSpan lapTime)
    {
        return _context.FastestLaps.FirstOrDefault(l => l.LapTime == lapTime);
    }
    
    public IEnumerable<FastestLap> ReadAllFastestLaps()
    {
        return _context.FastestLaps;
    }
    
    public IEnumerable<FastestLap> ReadFastestLapsByCircuit(string circuit)
    {
        return _context.FastestLaps.Where(lap => lap.Circuit == circuit);
    }
    
    public void CreateFastestLap(FastestLap lap)
    {
        _context.FastestLaps.Add(lap);
        _context.SaveChanges();  
    }
    
    public F1Car ReadF1Car(int id)
    {
        return _context.F1Cars.Find(id);
    }
    
    public IEnumerable<F1Car> ReadAllF1Cars()
    {
        return _context.F1Cars;
    }
    
    public IEnumerable<F1Car> ReadF1CarsByTeam(F1Team team)
    {
        return _context.F1Cars.Where(car => car.Team == team);
    }
    
    public void CreateF1Car(F1Car car)
    {
        _context.F1Cars.Add(car);
        _context.SaveChanges();  
    }
}