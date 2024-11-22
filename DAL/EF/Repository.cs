using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EF;

public class Repository(F1CarDbContext context) : IRepository
{
    public F1Car ReadF1Car(int id)
    {
        return context.F1Cars.Include(c => c.CarTyres).FirstOrDefault(car => car.Id == id);
    }
    
    public Race ReadRace(int id)
    {
        return context.Set<Race>().Include(r => r.FastestLaps).FirstOrDefault(r => r.Id == id);
    }
    
    public IEnumerable<FastestLap> ReadFastestLapsByTime(TimeSpan lapTime)
    {
        return context.FastestLaps
            .Include(f => f.Car)
            .Include(f => f.Race)
            .Where(lap => lap.LapTime == lapTime);
    }
    
    public IEnumerable<FastestLap> ReadAllFastestLaps()
    {
        return context.FastestLaps.Include(f => f.Car).Include(f => f.Race);
    }
    
    public IEnumerable<FastestLap> ReadFastestLapsByCircuit(string circuit)
    {
        return context.FastestLaps.Where(lap => lap.Circuit == circuit)
            .Include(f => f.Car)
            .Include(f => f.Race);
    }
    
    public IEnumerable<F1Car> ReadAllF1Cars()
    {
        return context.F1Cars.Include(c => c.CarTyres);
    }
    
    public IEnumerable<F1Car> ReadF1CarsByTeam(F1Team team)
    {
        return context.F1Cars.Where(car => car.Team == team).Include(c => c.CarTyres);
    }
    
    public IEnumerable<Race> ReadAllRaces()
    {
        return context.Set<Race>().Include(r => r.FastestLaps);
    }
    
    public IEnumerable<F1Car> ReadAllF1CarsWithTyresAndFastestLaps()
    {
        return context.F1Cars
            .Include(fc => fc.CarTyres)
            .Include(fc => fc.FastestLaps)
            .ThenInclude(fl => fl.Race) 
            .ToList();
    }
    
    public IEnumerable<Race> ReadAllRacesWithFastestLapsAndCars()
    {
        return context.Races
            .Include(r => r.FastestLaps)
            .ThenInclude(fl => fl.Car) 
            .ThenInclude(fc => fc.CarTyres) 
            .ToList();
    }
    
    public IEnumerable<CarTyre> ReadCarTyresForCarById(int carId)
    {
        return context.CarTyres.Where(ct => ct.Car.Id == carId).ToList();
    }
    
    public void AddCarTyre(CarTyre carTyre)
    {
        context.CarTyres.Add(carTyre);
        context.SaveChanges();
    }
    
    public void RemoveCarTyre(int carId, TyreType tyreType)
    {
        var carTyre = context.CarTyres
            .FirstOrDefault(ct => ct.Car.Id == carId && ct.Tyre == tyreType);

        if (carTyre != null)
        {
            context.CarTyres.Remove(carTyre);
            context.SaveChanges();
        }
    }
    
    public void CreateFastestLap(FastestLap lap)
    {
        context.FastestLaps.Add(lap);
        context.SaveChanges();
    }
    
    public void CreateF1Car(F1Car car)
    {
        context.F1Cars.Add(car);
        context.SaveChanges();
    }
    
    public void CreateRace(Race race)
    {
        context.Set<Race>().Add(race);
        context.SaveChanges();
    }
    
}