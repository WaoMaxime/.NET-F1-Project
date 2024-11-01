using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class FastestLap
    {
        [Key]
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
        
        public F1Car Car { get; set; }

        public int CarId { get; set; }  
        
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

        public FastestLap()
        {
            Circuit = "Dummy";
            AirTemperature = 20;
            TrackTemperature = 20;
            DateTime.Now.AddDays(-10); 
            LapTime = new TimeSpan(0, 1, 19, 567);
            Car = new F1Car();
        }
    }

}