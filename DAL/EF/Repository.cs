using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EF;

public class Repository : IRepository
{
    private readonly F1CarDbContext _context;

    public Repository(F1CarDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public F1Car ReadF1Car(int id)
    {
        return _context.F1Cars.Include(c => c.CarTyres).FirstOrDefault(car => car.Id == id);
    }
    
    public Race ReadRace(int id)
    {
        return _context.Set<Race>().Include(r => r.FastestLaps).FirstOrDefault(r => r.Id == id);
    }
    
    public IEnumerable<FastestLap> ReadFastestLapsByTime(TimeSpan lapTime)
    {
        return _context.FastestLaps
            .Include(f => f.Car)
            .Include(f => f.Race)
            .Where(lap => lap.LapTime == lapTime);
    }
    
    public IEnumerable<FastestLap> ReadAllFastestLaps()
    {
        return _context.FastestLaps.Include(f => f.Car).Include(f => f.Race);
    }
    
    public IEnumerable<FastestLap> ReadFastestLapsByCircuit(string circuit)
    {
        return _context.FastestLaps.Where(lap => lap.Circuit == circuit)
            .Include(f => f.Car)
            .Include(f => f.Race);
    }
    
    public IEnumerable<F1Car> ReadAllF1Cars()
    {
        return _context.F1Cars.Include(c => c.CarTyres);
    }
    
    public IEnumerable<F1Car> ReadF1CarsByTeam(F1Team team)
    {
        return _context.F1Cars.Where(car => car.Team == team).Include(c => c.CarTyres);
    }
    
    public IEnumerable<Race> ReadAllRaces()
    {
        return _context.Set<Race>().Include(r => r.FastestLaps);
    }
    
    public IEnumerable<F1Car> ReadAllF1CarsWithTyresAndFastestLaps()
    {
        return _context.F1Cars
            .Include(fc => fc.CarTyres)
            .Include(fc => fc.FastestLaps)
            .ThenInclude(fl => fl.Race);
    }
    
    public IEnumerable<Race> ReadAllRacesWithFastestLapsAndCars()
    {
        return _context.Races
            .Include(r => r.FastestLaps)
            .ThenInclude(fl => fl.Car)
            .ThenInclude(fc => fc.CarTyres);
    }
    
    public F1Car ReadF1CarWithDetails(int id)
    {
        return _context.F1Cars
            .Include(fc => fc.CarTyres) 
            .ThenInclude(ct => ct.Race) 
            .FirstOrDefault(fc => fc.Id == id);
    }
    
    public CarTyre ReadTyreById(int id)
    {
        return _context.CarTyres
            .Include(ct => ct.Car) 
            .FirstOrDefault(ct => ct.CarId == id);
    }

    public void AddCarTyre(CarTyre carTyre)
    {
        _context.CarTyres.Add(carTyre);
        _context.SaveChanges();
    }
    
    public void RemoveCarTyre(int carId, TyreType tyreType)
    {
        var carTyre = _context.CarTyres
            .FirstOrDefault(ct => ct.Car.Id == carId && ct.Tyre == tyreType);

        if (carTyre == null) return;
        _context.CarTyres.Remove(carTyre);
        _context.SaveChanges();
    }
    
    public void CreateFastestLap(FastestLap lap)
    {
        _context.FastestLaps.Add(lap);
        _context.SaveChanges();
    }
    
    public void CreateF1Car(F1Car car)
    {
        _context.F1Cars.Add(car);
        _context.SaveChanges();
    }
    
    public void CreateRace(Race race)
    {
        _context.Set<Race>().Add(race);
        _context.SaveChanges();
    }
}