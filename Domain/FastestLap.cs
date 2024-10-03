namespace Domain
{
    public class FastestLap
    {
        public string Circuit { get; set; }
        public int AirTemperature { get; set; }
        public int TrackTemperature { get; set; }
        public TimeSpan LapTime { get; set; }
        public DateTime DateOfRecord { get; set; }
        public F1Car Car { get; set; } 

        public FastestLap(string circuit, int airTemperature, int trackTemperature, TimeSpan lapTime, DateTime dateOfRecord, F1Car car)
        {
            Circuit = circuit;
            AirTemperature = airTemperature;
            TrackTemperature = trackTemperature;
            LapTime = lapTime;
            DateOfRecord = dateOfRecord;
            Car = car;
        }

        public override string ToString()
        {
            string formattedLapTime = $"{LapTime.Minutes:D2}:{LapTime.Seconds:D2}.{LapTime.Milliseconds:D3}";
            string formattedDate = DateOfRecord.ToString("dd MMM yyyy");
            return $"Lap at {Circuit} with a {formattedLapTime} by {Car.Chasis} of team {Car.Team}, using a {Car.Tyres} tyre compound driven at {formattedDate}, Under the conditions of AirTemp: {AirTemperature} and TrackTemp: {TrackTemperature}";
        }
    }
}