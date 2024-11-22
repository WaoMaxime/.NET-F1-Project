using System.ComponentModel.DataAnnotations;
using BusinessLayer;
using Domain;
using UI_CA.Extentions;
namespace UI_CA;

public class ConsoleUi(IManager manager)
{
    public void Run()
    {
        do
        {
            Console.WriteLine("What would you like to do?" +
                              "\n==========================" +
                              "\n0) Quit" +
                              "\n1) Show all team's car's" +
                              "\n2) Show fastest laps of team" +
                              "\n3) Show all tyre types" +
                              "\n4) Show fastest lap from circuit and/or the driven lap time" +
                              "\n5) Add a new F1 Car" +
                              "\n6) Add a new Fastest Lap" +
                              "\n7) Add tyre to car" +
                              "\n8) Remove tyre from car" +
                              "\nChoice (0-8):");

            switch (Console.ReadLine())
            {
                case "1":
                    ShowAllTeams();
                    break;
                case "2":
                    ShowFastestLapsByTeam();
                    break;
                case "3":
                    ShowTyreTypes();
                    break;
                case "4":
                    FilterLaps();
                    break;
                case "5":
                    AddNewF1Car();
                    break;
                case "6":
                    AddNewFastestLap();
                    break;
                case "7":
                    AddTyreToCar();
                    break;
                case "8":
                    RemoveTyreFromCar();
                    break;
            }
        } while (Console.ReadLine() != "0");
    }

    private void AddTyreToCar()
    {
        try
        {
            var resume = true;
            Console.WriteLine("Enter Car ID:");
            if (!int.TryParse(Console.ReadLine(), out var carId))
            {
                Console.WriteLine("Invalid car ID.");
                return;
            }
            if (manager.GetF1Car(carId).CarTyres.Count != 0)
            {
                Console.WriteLine("Car is already has a tyre setup assigned");
                resume = false;
            }
            if (!resume)
            {
                return;
            }
            var car = manager.GetF1Car(carId);
            if (car == null)
            {
                Console.WriteLine("Car not found.");
                return;
            }

            Console.WriteLine("Enter Tyre Type (Soft, Medium, Hard, Inter, or Full wet):");
            var tyreInput = Console.ReadLine();
            if (!Enum.TryParse(tyreInput, true, out TyreType tyreType))
            {
                Console.WriteLine("Invalid tyre type.");
                return;
            }

            Console.WriteLine("Enter Tyre Pressure:");
            if (!int.TryParse(Console.ReadLine(), out var tyrePressure))
            {
                Console.WriteLine("Invalid tyre pressure.");
                return;
            }

            Console.WriteLine("Enter Operational Temperature:");
            if (!int.TryParse(Console.ReadLine(), out var operationalTemperature))
            {
                Console.WriteLine("Invalid operational temperature.");
                return;
            }

            manager.AddTyreToCar(carId, tyreType, tyrePressure, operationalTemperature);
            Console.WriteLine("Tyre added successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}. Please try again.");
        }
    }

    private void RemoveTyreFromCar()
    {
        try
        {
            var resume = true;
            Console.WriteLine("Enter Car ID:");
            
            if (!int.TryParse(Console.ReadLine(), out var carId))
            {
                Console.WriteLine("Invalid car ID.");
                return;
            }
            
            if (manager.GetF1Car(carId).CarTyres.Count == 0)
            {
                Console.WriteLine("Car is already has currently no tyre setup assigned");
                resume = false;
            }
            
            if (!resume)
            {
                return;
            }

            var car = manager.GetF1Car(carId);
            if (car == null)
            {
                Console.WriteLine("Car not found.");
                return;
            }

            Console.WriteLine("Enter Tyre Type to Remove (Soft, Medium, Hard, Inter, or Full wet):");
            var tyreInput = Console.ReadLine();
            if (!Enum.TryParse(tyreInput, true, out TyreType tyreType))
            {
                Console.WriteLine("Invalid tyre type.");
                return;
            }

            manager.RemoveTyreFromCar(carId, tyreType);
            Console.WriteLine("Tyre removed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}. Please try again.");
        }
    }

    private void ShowAllTeams()
    {
        var cars = manager.GetAllF1CarsWithDetails();
        var uniqueCars = new HashSet<string>();

        foreach (var car in cars)
        {
            if (!uniqueCars.Add(car.Chasis)) continue;
            Console.WriteLine(PrintExtentions.PrintF1CarDetails(car));
            foreach (var tyre in car.CarTyres)
            {
                Console.WriteLine($"  Tyre: {tyre.Tyre}, Pressure: {tyre.TyrePressure}, Temp: {tyre.OperationalTemperature}" + 
                                  "\n----------------------------------------------------------------------------------------------------------------------------------------------------------");
            }
        }
    }

    private void ShowFastestLapsByTeam()
    {
        Console.WriteLine("Enter (part of) a Team Name:");
        var teamName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(teamName))
        {
            Console.WriteLine("Team name cannot be empty. Please try again.");
            return;
        }

        var laps = manager.GetAllFastestLaps();
        var matchingLaps = laps.Where(lap => lap.Car.Team.ToString().Contains(teamName, StringComparison.OrdinalIgnoreCase)).ToList();

        if (matchingLaps.Count > 0)
        {
            foreach (var lap in matchingLaps)
            {
                Console.WriteLine(PrintExtentions.PrintFastestLapDetails(lap));
            }
        }
        else
        {
            Console.WriteLine($"No fastest laps found for team name containing: \"{teamName}\".");
        }
    }

    private static void ShowTyreTypes()
    {
        var tyreTypes = Enum.GetValues(typeof(TyreType));

        Console.WriteLine("Available Tyre Types:");
        foreach (TyreType tyre in tyreTypes)
        {
            Console.WriteLine(tyre.ToFriendlyString());
        }
    }

    private void FilterLaps()
    {
        var fastestLap = new FastestLap();
        string circuitName;
        string lapTimeInput;
        var lapTime = new TimeSpan();
        do
        {
            Console.WriteLine("Enter the Circuit of the lap or leave blank:");
            circuitName = Console.ReadLine();

            if (!string.IsNullOrEmpty(circuitName))
            {
                fastestLap.Circuit = circuitName;

                var validationResults = new List<ValidationResult>();
                var context = new ValidationContext(fastestLap);
                var isValid = Validator.TryValidateObject(fastestLap, context, validationResults, true);

                if (isValid)
                {
                    break;
                }
                Console.WriteLine(validationResults[0].ErrorMessage);
            }
            else
            {
                break;
            }
        } while (true);

        do
        {
            Console.WriteLine("Enter a Lap Time (in the format 'minutes.seconds.milliseconds', e.g., 1.40.800) or leave blank:");
            lapTimeInput = Console.ReadLine(); 
            if (!string.IsNullOrEmpty(lapTimeInput))
            {
                try
                {
                    var parts = lapTimeInput.Split('.');
                    if (parts.Length == 3 &&
                        int.TryParse(parts[0], out var minutes) &&
                        int.TryParse(parts[1], out var seconds) &&
                        int.TryParse(parts[2], out var milliseconds))
                    {
                        lapTime = new TimeSpan(0, 0, minutes, seconds, milliseconds);
                        break;
                    }
                    Console.WriteLine("Invalid format. Please try again.");
                }
                catch
                {
                    Console.WriteLine("Error parsing lap time. Please try again.");
                }
            }
            else
            {
                break;
            }
        } while (true);

        if (string.IsNullOrEmpty(circuitName) && !string.IsNullOrEmpty(lapTimeInput))
        {
            DisplayFastestLaps(() => manager.GetFastestLapByTime(lapTime));
        }

        if (string.IsNullOrEmpty(lapTimeInput) && !string.IsNullOrEmpty(circuitName))
        {
            DisplayFastestLaps(() => manager.GetFastestLapsByCircuit(circuitName));
        }

        
        if (string.IsNullOrEmpty(circuitName) && string.IsNullOrEmpty(lapTimeInput))
        {
            DisplayFastestLaps(manager.GetAllFastestLaps);
        }

    }
    
    private void AddNewF1Car()
    {
        try
        {
            var newCar = new F1Car();
            do
            {
                Console.WriteLine("Enter Team (e.g., Mercedes, RedBull, Ferrari):");
                var teamInput = Console.ReadLine(); 
                if (string.IsNullOrWhiteSpace(teamInput))
                { 
                    Console.WriteLine("You entered a blank team. Please enter a team name"); 
                    continue;
                }
                if (Enum.TryParse(teamInput, true, out F1Team teamName) && Enum.IsDefined(typeof(F1Team), teamName)) 
                { 
                    newCar.Team = teamName;
                    var validationResults = new List<ValidationResult>(); 
                    var context = new ValidationContext(newCar); 
                    var isValid = Validator.TryValidateObject(newCar, context, validationResults, true);

                    if (isValid)
                    {
                        break;
                    }
                    Console.WriteLine(validationResults[0].ErrorMessage);
                }
                else
                { 
                    Console.WriteLine("Invalid team name. Please enter a valid F1 team.");
                }
            } while (true);
                        
            do
            {
                Console.WriteLine("Enter Chassis Name:");
                newCar.Chasis = Console.ReadLine();
                    
                var validationResults = new List<ValidationResult>();
                var context = new ValidationContext(newCar);
                var isValid = Validator.TryValidateObject(newCar, context, validationResults, true);

                if (isValid)
                { 
                    break;
                }
                Console.WriteLine(validationResults[0].ErrorMessage);
            } while (true);
                        
            do
            {
                Console.WriteLine("Enter Constructors Position (1-10):"); 
                if (!int.TryParse(Console.ReadLine(), out var constructorsPosition))
                { 
                    Console.WriteLine("Please enter a valid number of constructors positions.");
                }

                newCar.ConstructorsPosition = constructorsPosition;

                var validationResults = new List<ValidationResult>();
                var context = new ValidationContext(newCar);
                var isValid = Validator.TryValidateObject(newCar, context, validationResults, true);

                if (isValid)
                { 
                    break;
                } 
                Console.WriteLine(validationResults[0].ErrorMessage);
            } while (true);
                        
            do
            {
                Console.WriteLine("Enter Driver's Position in Championship (1-20):");
                if (!double.TryParse(Console.ReadLine(), out var driverPosition))
                { 
                    Console.WriteLine("Please enter a valid number of driver's position.");
                }

                newCar.DriversPositions = driverPosition;
                var validationResults = new List<ValidationResult>();
                var context = new ValidationContext(newCar);
                var isValid = Validator.TryValidateObject(newCar, context, validationResults, true);

                if (isValid)
                { 
                    break;
                }
                Console.WriteLine(validationResults[0].ErrorMessage);
            } while (true);
                        
            do
            {
                Console.WriteLine("Enter Manufacture Date (yyyy-mm-dd):");
                var dateOfManufactureInput = Console.ReadLine();
                if (!DateTime.TryParse(dateOfManufactureInput, out var dateOfManufacture) || dateOfManufacture > DateTime.Now)
                { 
                    Console.WriteLine("You didn't enter a valid date. Please enter a date in format 'yyyy-mm-dd'."); 
                    continue;
                }
                    
                newCar.ManufactureDate = dateOfManufacture;

                var validationResults = new List<ValidationResult>();
                var context = new ValidationContext(newCar);
                var isValid = Validator.TryValidateObject(newCar, context, validationResults, true);

                if (isValid)
                { 
                    break;
                }
                Console.WriteLine(validationResults[0].ErrorMessage);
            } while (true);
                        
            do
            {
                Console.WriteLine("Enter Tyre Type (Soft, Medium, Hard, Inter or Full wet):");
                var tyreInput = Console.ReadLine();

                if (Enum.TryParse(tyreInput, true, out TyreType tyres)) 
                {
                    newCar.Tyres = tyres;

                    var validationResults = new List<ValidationResult>();
                    var context = new ValidationContext(newCar);
                    var isValid = Validator.TryValidateObject(newCar, context, validationResults, true);

                    if (isValid)
                    { 
                        break;
                    }
                    Console.WriteLine(validationResults[0].ErrorMessage);
                }
                else
                {
                    Console.WriteLine("Invalid tyre type. Please enter 'Soft', 'Medium', or 'Hard'.");
                }
            } while (true);
                        
            do
            {
                Console.WriteLine("Enter Engine Power (in HP, e.g., 1000):");
                if (double.TryParse(Console.ReadLine(), out var enginePower) && enginePower is >= 500 and <= 1500)
                { 
                    newCar.EnginePower = enginePower;

                    var validationResults = new List<ValidationResult>();
                    var context = new ValidationContext(newCar);
                    var isValid = Validator.TryValidateObject(newCar, context, validationResults, true);

                    if (isValid)
                    { 
                        break;
                    }
                    Console.WriteLine(validationResults[0].ErrorMessage);
                }
                else
                {
                    Console.WriteLine("Invalid engine power. Please enter a value between 500 and 1500.");
                } 
            } while (true);
            manager.AddF1Car(newCar.Team, newCar.Chasis,newCar.ConstructorsPosition,newCar.DriversPositions,newCar.ManufactureDate,newCar.Tyres,newCar.EnginePower);
            Console.WriteLine($"New F1Car Added: {PrintExtentions.PrintF1CarDetails(newCar)}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}. Please try again.");
        }
    }
        

    private void AddNewFastestLap() 
    {
        try
        {
            var lap = new FastestLap();
            string circuit;
            int airTemperature, trackTemperature;
            var lapTime = TimeSpan.Zero;
            DateTime dateOfRecord;
            var exit = false;
                
            do
            {
                Console.WriteLine("Enter Circuit Name (min 3 characters, max 50):");
                circuit = Console.ReadLine();
                var circuitOnly = new FastestLap { Circuit = circuit };
                var validationResults = new List<ValidationResult>();
                var circuitContext = new ValidationContext(circuitOnly);

                var isValid = Validator.TryValidateObject(circuitOnly, circuitContext, validationResults, true);

                if (!isValid)
                {
                    Console.WriteLine(validationResults[0].ErrorMessage);
                }
                else
                {
                    lap.Circuit = circuitOnly.Circuit;
                    break;
                }
            } while (true);
                
            do
            {
                Console.WriteLine("Enter Air Temperature (between -30 and 50 degrees Celsius):");
                var airTempInput = Console.ReadLine();

                if (!int.TryParse(airTempInput, out airTemperature))
                {
                    Console.WriteLine("You didn't enter a valid number. Please enter a number.");
                    continue;
                }

                var tempLap = new FastestLap { AirTemperature = airTemperature };
                var validationResults = new List<ValidationResult>();  
                var context = new ValidationContext(tempLap);
                tempLap.Circuit = circuit;

                var isValid = Validator.TryValidateObject(tempLap, context, validationResults, true);

                if (!isValid)
                {
                    Console.WriteLine(validationResults[0].ErrorMessage);
                }
                else
                {
                    lap.AirTemperature = tempLap.AirTemperature;
                    break;
                }
            } while (true);
                
            do
            {
                Console.WriteLine("Enter Track Temperature (between -30 and 50 degrees Celsius):");
                var trackTempInput = Console.ReadLine();

                if (!int.TryParse(trackTempInput, out trackTemperature))
                {
                    Console.WriteLine("You didn't enter a valid number. Please enter a number.");
                    continue;
                }

                var trackTemp = new FastestLap { TrackTemperature = trackTemperature };
                var validationResults = new List<ValidationResult>();  
                var context = new ValidationContext(trackTemp);
                trackTemp.AirTemperature = airTemperature;

                var isValid = Validator.TryValidateObject(trackTemp, context, validationResults, true);

                if (!isValid)
                {
                    Console.WriteLine(validationResults[0].ErrorMessage);
                }
                else
                {
                    lap.TrackTemperature = trackTemp.TrackTemperature;
                    break;
                }
            } while (true);
                
            do
            {
                Console.WriteLine("Enter Lap Time (format 'm.sss.ms ms ms', e.g., 1.40.564):");
                var lapTimeInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(lapTimeInput))
                {
                    var timeLap = new FastestLap { LapTime = lapTime };
                    var validationResults = new List<ValidationResult>();
                    var context = new ValidationContext(timeLap);
                    timeLap.TrackTemperature = trackTemperature;
                    var isValid = Validator.TryValidateObject(timeLap, context, validationResults, true);

                    if (!isValid)
                    {
                        Console.WriteLine(validationResults[0].ErrorMessage); 
                    }
                }
                else
                {
                    var parts = lapTimeInput.Split('.');
                    if (parts.Length == 3 &&
                        int.TryParse(parts[0], out var minutes) &&
                        int.TryParse(parts[1], out var seconds) &&
                        int.TryParse(parts[2], out var milliseconds))
                    {
                        if (seconds < 60 && milliseconds < 1000)
                        {
                            lapTime = new TimeSpan(0, 0, minutes, seconds, milliseconds);
                            lap.LapTime = lapTime;
                            break; 
                        }
                        Console.WriteLine("Seconds must be less than 60, milliseconds less than 1000.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid format. Please enter the lap time in 'm.sss.ms ms ms' format.");
                    }
                }
            } while (true);
                
            do
            {
                Console.WriteLine("Enter Date of Record (yyyy-mm-dd):");
                var dateOfRecordInput = Console.ReadLine();
                if (!DateTime.TryParse(dateOfRecordInput, out dateOfRecord) || dateOfRecord > DateTime.Now)
                {
                    Console.WriteLine("You didn't enter a valid date. Please enter a date in format 'yyyy-mm-dd'.");
                    continue;
                }
                    
                var recordDate = new FastestLap { DateOfRecord = dateOfRecord };
                var validationResults = new List<ValidationResult>();
                var context = new ValidationContext(recordDate);
                recordDate.LapTime = lapTime;
                    
                var isValid = Validator.TryValidateObject(recordDate, context, validationResults, true);

                if (!isValid)
                { 
                    Console.WriteLine(validationResults[0].ErrorMessage);
                }
                else
                { 
                    lap.DateOfRecord = recordDate.DateOfRecord; 
                    break;
                }
            } while (true);
                
            do
            {
                Console.WriteLine("Enter Race Name:");
                string raceName = Console.ReadLine();

                var allRaces = manager.GetAllRaces()
                    .Where(r => r.Name.Equals(raceName, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                if (allRaces.Count == 0)
                {
                    Console.WriteLine("No race found with that name. Please try again or add a new race first by adding the car tyre setup.");
                    Console.WriteLine("Type 'exit' to exit or press Enter to try again.");
                    var userInput = Console.ReadLine();
                    if (!string.IsNullOrEmpty(userInput) && userInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        exit = true;
                        break;
                    }
                    continue; 
                }
                var race = allRaces.First();
                lap.Race = race;
                break; 
            } while (true);

            if (exit)
            { 
                return;
            }
            var newLap = manager.AddFastestLap(circuit, airTemperature, trackTemperature, lapTime, dateOfRecord, lap.Car, lap.Race);
            Console.WriteLine($"New Fastest Lap Added: {PrintExtentions.PrintFastestLapDetails(newLap)}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}. Please try again.");
        }
    }

    private static void DisplayFastestLaps(Func<IEnumerable<FastestLap>> getLapsMethod)
    {
        var laps = getLapsMethod().ToList();
        if (laps.Count != 0 == false)
        {
            Console.WriteLine("No fastest laps found.");
            return;
        }
        foreach (var lap in laps)
        {
            Console.WriteLine(PrintExtentions.PrintFastestLapDetails(lap));
        }
    }
}