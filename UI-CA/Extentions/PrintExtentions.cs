using BusinessLayer;
using Domain;

namespace UI_CA.Extentions;

public static class PrintExtentions
{
    private static readonly IManager _manager;
    
    public static String PrintF1CarDetails(F1Car car)
    {
        return
            string.Format("Team {0} with chassis {1}, Constructors Position: {2}, Drivers Position: {3}, Manufactured on: {4:dd-MM-yyyy}, Engine Power: {5} HP",
                car.Team, car.Chasis, car.ConstructorsPosition, car.DriversPositions, car.ManufactureDate,
                car.EnginePower.HasValue ? car.EnginePower.ToString() : "N/A") +
            "\n--------------------------------------------------------------------------------------------------------------------------------------";
    }

    public static String PrintFastestLapDetails(FastestLap lap)
    {
        string formattedLapTime = $"{lap.LapTime.Minutes:D2}:{lap.LapTime.Seconds:D2}.{lap.LapTime.Milliseconds:D3}";
        string formattedDate = lap.DateOfRecord.ToString("dd MMM yyyy");
        return
            $"Lap at {lap.Circuit} with a {formattedLapTime} by {_manager.GetF1Car(lap.CarId).Chasis} of team {_manager.GetF1Car(lap.CarId).Team}, using a {_manager.GetF1Car(lap.CarId).Tyres} tyre compound driven at {formattedDate}, Under the conditions of AirTemp: {lap.AirTemperature} and TrackTemp: {lap.TrackTemperature}";
    }
}