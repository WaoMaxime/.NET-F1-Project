namespace Sprint1;

public class FastestLap
{
    public string Circuit { get; set; }
    public int AirTemperature { get; set; }
    public int TrackTemperature { get; set; }
    public TimeSpan LapTime { get; set; }
    public DateTime DateOfRecord { get; set; }

    public FastestLap(TimeSpan lapTime, DateTime dateOfRecord)
    {
        LapTime = lapTime;
        DateOfRecord = dateOfRecord;
    }

    public override string ToString()
    {
        string formattedLapTime = $"{LapTime.Minutes:D2}:{LapTime.Seconds:D2}.{LapTime.Milliseconds:D3}";
        
        string formattedDate = DateOfRecord.ToString("dd MMM yyyy");

       return $"Lap Time: {formattedLapTime} | Date: {formattedDate}";
    }

    
    
}