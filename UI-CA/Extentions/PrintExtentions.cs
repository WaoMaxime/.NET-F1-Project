using Domain;

namespace UI_CA.Extentions;

public static class PrintExtentions 
{
    public static string PrintF1CarDetails(F1Car car)
    {
        return
            $"Team {car.Team} with chassis {car.Chasis}, Constructors Position: {car.ConstructorsPosition}, Drivers Position: {car.DriversPositions}, Manufactured on: {car.ManufactureDate:dd-MM-yyyy}, Engine Power: {(car.EnginePower.HasValue ? car.EnginePower.ToString() : "N/A")} HP";
    }
    public static string PrintFastestLapDetails(FastestLap lap)
    {
        var formattedLapTime = $"{lap.LapTime.Minutes:D2}:{lap.LapTime.Seconds:D2}.{lap.LapTime.Milliseconds:D3}";
        var formattedDate = lap.DateOfRecord.ToString("dd MMM yyyy");
        return
            $"Lap at {lap.Circuit} with a {formattedLapTime} by {lap.Car.Chasis} of team {lap.Car.Team.ToString()} during the {lap.Race.Name},\n using a {lap.Car.Tyres.ToString()} tyre compound driven at {formattedDate}, Under the conditions of AirTemp: {lap.AirTemperature} and TrackTemp: {lap.TrackTemperature}" +
            "\n----------------------------------------------------------------------------------------------------------------------------------------------------------";
    }
}