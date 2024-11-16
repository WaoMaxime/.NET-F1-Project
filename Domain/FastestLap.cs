using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class FastestLap
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Circuit name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Circuit name must be between 3 and 50 characters.")]
        public string Circuit { get; set; } = null!;

        [Range(-30, 50, ErrorMessage = "Air temperature must be between -30 and 50 degrees Celsius.")]
        public int AirTemperature { get; set; }

        [Range(-30, 70, ErrorMessage = "Track temperature must be between -30 and 70 degrees Celsius.")]
        public int TrackTemperature { get; set; }

        [Required(ErrorMessage = "Lap time is required.")]
        public TimeSpan LapTime { get; set; }

        [Required(ErrorMessage = "Date of record is required.")]
        public DateTime DateOfRecord { get; set; }

        [Required(ErrorMessage = "An existing car is required.")]
        public F1Car Car { get; set; } = null!;

        public int CarId { get; set; }

        [Required(ErrorMessage = "Race is required.")]
        public Race Race { get; set; } = null!;

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
}
