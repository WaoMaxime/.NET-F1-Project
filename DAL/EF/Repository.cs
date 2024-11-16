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
        return _context.FastestLaps
            .Include(f => f.Car)
            .Include(f => f.Race)
            .FirstOrDefault(l => l.LapTime == lapTime);
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

    public void CreateFastestLap(FastestLap lap)
    {
        _context.FastestLaps.Add(lap);
        _context.SaveChanges();
    }
    
    public IEnumerable<Race> ReadAllRaces()
    {
        return _context.Set<Race>().Include(r => r.FastestLaps);
    }

    public Race ReadRace(int id)
    {
        return _context.Set<Race>().Include(r => r.FastestLaps).FirstOrDefault(r => r.Id == id);
    }

    public IEnumerable<F1Car> ReadAllF1CarsWithTyresAndFastestLaps()
    {
        return _context.F1Cars
            .Include(fc => fc.CarTyres)
            .Include(fc => fc.FastestLaps)
            .ThenInclude(fl => fl.Race) 
            .ToList();
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

        if (carTyre != null)
        {
            _context.CarTyres.Remove(carTyre);
            _context.SaveChanges();
        }
    }
    
    public IEnumerable<CarTyre> ReadCarTyresForCar(int carId)
    {
        return _context.CarTyres.Where(ct => ct.Car.Id == carId).ToList();
    }
    
    public IEnumerable<Race> ReadAllRacesWithFastestLapsAndCars()
    {
        return _context.Races
            .Include(r => r.FastestLaps)
            .ThenInclude(fl => fl.Car) 
            .ThenInclude(fc => fc.CarTyres) 
            .ToList();
    }

    public void CreateRace(Race race)
    {
        _context.Set<Race>().Add(race);
        _context.SaveChanges();
    }
    
    public F1Car ReadF1Car(int id)
    {
        return _context.F1Cars.Include(c => c.CarTyres).FirstOrDefault(car => car.Id == id);
    }

    public IEnumerable<F1Car> ReadAllF1Cars()
    {
        return _context.F1Cars.Include(c => c.CarTyres);
    }

    public IEnumerable<F1Car> ReadF1CarsByTeam(F1Team team)
    {
        return _context.F1Cars.Where(car => car.Team == team).Include(c => c.CarTyres);
    }

    public void CreateF1Car(F1Car car)
    {
        _context.F1Cars.Add(car);
        _context.SaveChanges();
    }
}