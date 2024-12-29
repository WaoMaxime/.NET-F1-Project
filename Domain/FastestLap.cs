using System.ComponentModel.DataAnnotations;
namespace Domain;

public class FastestLap
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Circuit { get; set; }

    [Range(-30, 50)]
    public int AirTemperature { get; set; }

    [Range(-30, 70)]
    public int TrackTemperature { get; set; }

    [Required]
    public TimeSpan LapTime { get; set; }
        
    public DateTime DateOfRecord { get; set; }
        
    public F1Car Car { get; set; }

    public int CarId { get; set; }
        
    public Race Race { get; set; }

    public int RaceId { get; set; }
        
    public FastestLap(string circuit, int airTemperature, int trackTemperature, TimeSpan lapTime,
        DateTime dateOfRecord, F1Car car, Race race)
    {
        Circuit = circuit;
        AirTemperature = airTemperature;
        TrackTemperature = trackTemperature;
        LapTime = lapTime;
        DateOfRecord = dateOfRecord;
        Car = car;
        Race = race;
        CarId = car.Id;
        RaceId = race.Id;
    }
        
    public FastestLap()
    {
        Circuit = "Default Circuit";
        AirTemperature = 20; 
        TrackTemperature = 30; 
        LapTime = new TimeSpan(0, 1, 30); 
        DateOfRecord = DateTime.Now.AddDays(-7); 
        Car = new F1Car();
        CarId = Car.Id; 
        Race = new Race { Name = "Default Race", Date = DateTime.Now.AddDays(-7) }; 
        RaceId = Race.Id; 
    }
}