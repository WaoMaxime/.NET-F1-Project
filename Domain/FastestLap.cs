using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class FastestLap
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Circuit name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Circuit name must be between 3 and 50 characters.")]
        public string Circuit { get; set; }

        [Range(-30, 50, ErrorMessage = "Air temperature must be between -30 and 50 degrees Celsius.")]
        public int AirTemperature { get; set; }

        [Range(-30, 70, ErrorMessage = "Track temperature must be between -30 and 70 degrees Celsius.")]
        public int TrackTemperature { get; set; }

        [Required(ErrorMessage = "Lap time is required.")]
        public TimeSpan LapTime { get; set; }

        [Required(ErrorMessage = "Date of record is required.")]
        public DateTime DateOfRecord { get; set; }

        [Required(ErrorMessage = "A car is required for the fastest lap.")]
        
        [NotMapped]  
        public F1Car Car { get; set; }

        public int CarId { get; set; } 
        

        public FastestLap() { }
    
        public FastestLap(string circuit, int airTemperature, int trackTemperature, TimeSpan lapTime,
            DateTime dateOfRecord, F1Car car)
        {
            Circuit = circuit;
            AirTemperature = airTemperature;
            TrackTemperature = trackTemperature;
            LapTime = lapTime;
            DateOfRecord = dateOfRecord;
            Car = car;
            CarId = car.Id;  
        }
    }

}