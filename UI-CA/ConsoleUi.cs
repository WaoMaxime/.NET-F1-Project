using System.ComponentModel.DataAnnotations;
using BusinessLayer;
using Domain;
using UI_CA.Extentions;

namespace UI_CA
{
    public class ConsoleUi
    {
        private readonly IManager _manager;
        public ConsoleUi(IManager manager)
        {
            _manager = manager;
        }

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
                                  "\n4) Show fastest lap from circuit and/or the driven laptime" +
                                  "\n5) Add a new F1 Car" +
                                  "\n6) Add a new Fastest Lap" +
                                  "\nChoice (0-6):");
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
                }
            } while (Console.ReadLine() != "0");
        }

        private void ShowAllTeams()
        {
            var cars = _manager.GetAllF1Cars();
            var uniqueCars = new HashSet<string>();

            foreach (var car in cars)
            {
                if (uniqueCars.Add(car.Chasis))
                {
                    Console.WriteLine(PrintExtentions.PrintF1CarDetails(car));
                }
            }
        }

        private void ShowFastestLapsByTeam()
        {
            Console.WriteLine("Enter (part of) a Team Name:");
            string teamName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(teamName))
            {
                Console.WriteLine("Team name cannot be empty. Please try again.");
                return;
            }

            var laps = _manager.GetAllFastestLaps();
            var matchingLaps = new List<FastestLap>();

            foreach (var lap in laps)
            {
                if (lap.Car.Team.ToString().IndexOf(teamName, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    matchingLaps.Add(lap);
                }
            }
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



        public void ShowTyreTypes()
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
            
            FastestLap fastestLap = new FastestLap();
            string circuitName;
            string lapTimeInput;
            TimeSpan lapTime = new TimeSpan();
            do
            {
                Console.WriteLine("Enter the Circuit of the lap or leave blank:");
                circuitName = Console.ReadLine();

                if (!string.IsNullOrEmpty(circuitName)) 
                {
                    fastestLap.Circuit = circuitName;

                    var validationResults = new List<ValidationResult>();
                    var context = new ValidationContext(fastestLap);
                    bool isValid = Validator.TryValidateObject(fastestLap, context, validationResults, true);

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
                    string[] parts = lapTimeInput.Split('.');
                    int.TryParse(parts[0], out int minutes);
                    int.TryParse(parts[1], out int seconds);
                    int.TryParse(parts[2], out int milliseconds);
                    lapTime = new TimeSpan(0, 0, minutes, seconds, milliseconds);

                    var validationResults = new List<ValidationResult>();
                    var context = new ValidationContext(lapTime);
                    bool isValid = Validator.TryValidateObject(lapTime, context, validationResults, true);

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

            if (circuitName == String.Empty && lapTimeInput != String.Empty)
            {
                if (_manager.GetFastestLapByTime(lapTime) == null)
                {
                    Console.WriteLine("No fastest laps found.");
                    return;
                }
                Console.WriteLine(PrintExtentions.PrintFastestLapDetails(_manager.GetFastestLapByTime(lapTime)));
            }
                
            if (lapTimeInput == String.Empty && circuitName != String.Empty)
            {
                var laps = _manager.GetFastestLapsByCircuit(circuitName);
                if (laps.Any() == false)
                {
                    Console.WriteLine("No fastest laps found.");
                    return;
                }
                foreach (var lap in laps)
                {
                    Console.WriteLine(PrintExtentions.PrintFastestLapDetails(lap));
                }
            }
            
            if (circuitName == String.Empty && lapTimeInput == String.Empty)
            {
                var laps = _manager.GetAllFastestLaps();
                if (laps.Any() == false)
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
        private void AddNewF1Car()
        {
            try
            {
                F1Car newCar = new F1Car();
                        
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
                        bool isValid = Validator.TryValidateObject(newCar, context, validationResults, true);

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
                    bool isValid = Validator.TryValidateObject(newCar, context, validationResults, true);

                    if (isValid)
                    { 
                        break;
                    }
                    Console.WriteLine(validationResults[0].ErrorMessage);
                } while (true);
                        
                do
                {
                    Console.WriteLine("Enter Constructors Position (1-10):"); 
                    if (!int.TryParse(Console.ReadLine(), out int constructorsPosition))
                    { 
                        Console.WriteLine("Please enter a valid number of constructors positions.");
                    }

                    newCar.ConstructorsPosition = constructorsPosition;

                    var validationResults = new List<ValidationResult>();
                    var context = new ValidationContext(newCar);
                    bool isValid = Validator.TryValidateObject(newCar, context, validationResults, true);

                    if (isValid)
                    { 
                        break;
                    } 
                    Console.WriteLine(validationResults[0].ErrorMessage);
                } while (true);
                        
                do
                {
                    Console.WriteLine("Enter Driver's Position in Championship (1-20):");
                    if (!double.TryParse(Console.ReadLine(), out double driverPosition))
                    { 
                        Console.WriteLine("Please enter a valid number of driver's position.");
                    }

                    newCar.DriversPositions = driverPosition;
                    var validationResults = new List<ValidationResult>();
                    var context = new ValidationContext(newCar);
                    bool isValid = Validator.TryValidateObject(newCar, context, validationResults, true);

                    if (isValid)
                    { 
                        break;
                    }
                    Console.WriteLine(validationResults[0].ErrorMessage);
                } while (true);
                        
                do
                {
                    Console.WriteLine("Enter Manufacture Date (yyyy-mm-dd):");
                    string dateOfManufactureInput = Console.ReadLine();
                    if (!DateTime.TryParse(dateOfManufactureInput, out DateTime dateOfManufacture) || dateOfManufacture > DateTime.Now)
                    { 
                        Console.WriteLine("You didn't enter a valid date. Please enter a date in format 'yyyy-mm-dd'."); 
                        continue;
                    }
                    
                    newCar.ManufactureDate = dateOfManufacture;

                    var validationResults = new List<ValidationResult>();
                    var context = new ValidationContext(newCar);
                    bool isValid = Validator.TryValidateObject(newCar, context, validationResults, true);

                    if (isValid)
                    { 
                        break;
                    }
                    Console.WriteLine(validationResults[0].ErrorMessage);
                } while (true);
                        
                do
                {
                    Console.WriteLine("Enter Tyre Type (Soft, Medium, Hard, Inter or Fullwet):");
                    string tyreInput = Console.ReadLine();

                    if (Enum.TryParse(tyreInput, true, out TyreType tyres)) 
                    {
                        newCar.Tyres = tyres;

                        var validationResults = new List<ValidationResult>();
                        var context = new ValidationContext(newCar);
                        bool isValid = Validator.TryValidateObject(newCar, context, validationResults, true);

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
                    if (double.TryParse(Console.ReadLine(), out double enginePower) && enginePower >= 500 && enginePower <= 1500)
                    { 
                        newCar.EnginePower = enginePower;

                        var validationResults = new List<ValidationResult>();
                        var context = new ValidationContext(newCar);
                        bool isValid = Validator.TryValidateObject(newCar, context, validationResults, true);

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
                Console.WriteLine($"New F1Car Added: {PrintExtentions.PrintF1CarDetails(newCar)}");
                _manager.AddF1Car(newCar.Team, newCar.Chasis,newCar.ConstructorsPosition,newCar.DriversPositions,newCar.ManufactureDate,newCar.Tyres,newCar.EnginePower);
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
                FastestLap lap = new FastestLap();
                string circuit;
                int airTemperature, trackTemperature;
                TimeSpan lapTime = TimeSpan.Zero;
                DateTime dateOfRecord;
                
                do
                {
                    Console.WriteLine("Enter Circuit Name (min 3 characters, max 50):");
                    circuit = Console.ReadLine();
                    var circuitOnly = new FastestLap { Circuit = circuit };
                    var validationResults = new List<ValidationResult>();
                    var circuitContext = new ValidationContext(circuitOnly);

                    bool isValid = Validator.TryValidateObject(circuitOnly, circuitContext, validationResults, true);

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
                    string airTempInput = Console.ReadLine();

                    if (!int.TryParse(airTempInput, out airTemperature))
                    {
                        Console.WriteLine("You didn't enter a valid number. Please enter a number.");
                        continue;
                    }

                    var tempLap = new FastestLap { AirTemperature = airTemperature };
                    var validationResults = new List<ValidationResult>();  
                    var context = new ValidationContext(tempLap);
                    tempLap.Circuit = circuit;

                    bool isValid = Validator.TryValidateObject(tempLap, context, validationResults, true);

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
                    string trackTempInput = Console.ReadLine();

                    if (!int.TryParse(trackTempInput, out trackTemperature))
                    {
                        Console.WriteLine("You didn't enter a valid number. Please enter a number.");
                        continue;
                    }

                    var trackTemplap = new FastestLap { TrackTemperature = trackTemperature };
                    var validationResults = new List<ValidationResult>();  
                    var context = new ValidationContext(trackTemplap);
                    trackTemplap.AirTemperature = airTemperature;

                    bool isValid = Validator.TryValidateObject(trackTemplap, context, validationResults, true);

                    if (!isValid)
                    {
                        Console.WriteLine(validationResults[0].ErrorMessage);
                    }
                    else
                    {
                        lap.TrackTemperature = trackTemplap.TrackTemperature;
                        break;
                    }
                } while (true);
                
                do
                {
                    Console.WriteLine("Enter Lap Time (format 'm.sss.msmsms', e.g., 1.40.564):");
                    string lapTimeInput = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(lapTimeInput))
                    {
                        var timeLap = new FastestLap { LapTime = lapTime };
                        var validationResults = new List<ValidationResult>();
                        var context = new ValidationContext(timeLap);
                        timeLap.TrackTemperature = trackTemperature;

                        bool isValid = Validator.TryValidateObject(timeLap, context, validationResults, true);

                        if (!isValid)
                        {
                            Console.WriteLine(validationResults[0].ErrorMessage); 
                        }
                    }
                    else
                    {
                        string[] parts = lapTimeInput.Split('.');
                        if (parts.Length == 3 &&
                            int.TryParse(parts[0], out int minutes) &&
                            int.TryParse(parts[1], out int seconds) &&
                            int.TryParse(parts[2], out int milliseconds))
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
                            Console.WriteLine("Invalid format. Please enter the lap time in 'm.sss.msmsms' format.");
                        }
                    }
                } while (true);
                
                do
                {
                    Console.WriteLine("Enter Date of Record (yyyy-mm-dd):");
                    string lapTimeInput = Console.ReadLine();
                    if (!DateTime.TryParse(lapTimeInput, out dateOfRecord) || dateOfRecord > DateTime.Now)
                    {
                        Console.WriteLine("You didn't enter a valid date. Please enter a date in format 'yyyy-mm-dd'.");
                        continue;
                    }
                    
                    var recordDate = new FastestLap { DateOfRecord = dateOfRecord };
                    var validationResults = new List<ValidationResult>();
                    var context = new ValidationContext(recordDate);
                    recordDate.LapTime = lapTime;
                    
                    bool isValid = Validator.TryValidateObject(recordDate, context, validationResults, true);

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
                    Console.WriteLine("Enter the name of the car (or type 'new' to add a new car):");
                    string carInput = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(carInput))
                    {
                        Console.WriteLine("You didn't enter a valid name. Please enter a name or type 'new'.");
                        continue;
                    }

                    if (carInput.ToLower() == "new")
                    {
                        F1Car newCar = new F1Car();
                        
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
                                bool isValid = Validator.TryValidateObject(newCar, context, validationResults, true);

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
                            bool isValid = Validator.TryValidateObject(newCar, context, validationResults, true);

                            if (isValid)
                            {
                                break;
                            }
                            Console.WriteLine(validationResults[0].ErrorMessage);
                        } while (true);
                        
                        do
                        {
                            Console.WriteLine("Enter Constructors Position (1-10):");
                            if (!int.TryParse(Console.ReadLine(), out int constructorsPosition))
                            {
                                Console.WriteLine("Please enter a valid number of constructors positions.");
                            }

                            newCar.ConstructorsPosition = constructorsPosition;

                            var validationResults = new List<ValidationResult>();
                            var context = new ValidationContext(newCar);
                            bool isValid = Validator.TryValidateObject(newCar, context, validationResults, true);

                            if (isValid)
                            { 
                                break;
                            }
                            Console.WriteLine(validationResults[0].ErrorMessage);
                        } while (true);
                        
                        do
                        {
                            Console.WriteLine("Enter Driver's Position in Championship (1-20):");
                            if (!double.TryParse(Console.ReadLine(), out double driverPosition))
                            {
                                Console.WriteLine("Please enter a valid number of driver's position.");
                            }
                            newCar.DriversPositions = driverPosition;
                            var validationResults = new List<ValidationResult>();
                            var context = new ValidationContext(newCar);
                            bool isValid = Validator.TryValidateObject(newCar, context, validationResults, true);

                            if (isValid)
                            { 
                                break;
                            }
                            Console.WriteLine(validationResults[0].ErrorMessage);
                        } while (true);
                        
                        do
                        {
                            Console.WriteLine("Enter Manufacture Date (yyyy-mm-dd):");
                            string dateOfManufactureInput = Console.ReadLine();
                            if (!DateTime.TryParse(dateOfManufactureInput, out DateTime dateOfManufacture) || dateOfRecord > DateTime.Now)
                            {
                                Console.WriteLine("You didn't enter a valid date. Please enter a date in format 'yyyy-mm-dd'.");
                                continue;
                            }
                    
                            newCar.ManufactureDate = dateOfManufacture;

                            var validationResults = new List<ValidationResult>();
                            var context = new ValidationContext(newCar);
                            bool isValid = Validator.TryValidateObject(newCar, context, validationResults, true);

                            if (isValid)
                            {
                                break;
                            }
                            Console.WriteLine(validationResults[0].ErrorMessage);
                        } while (true);
                        
                        do
                        {
                            Console.WriteLine("Enter Tyre Type (Soft, Medium, Hard):");
                            string tyreInput = Console.ReadLine();

                            if (Enum.TryParse(tyreInput, true, out TyreType tyres))
                            {
                                newCar.Tyres = tyres;

                                var validationResults = new List<ValidationResult>();
                                var context = new ValidationContext(newCar);
                                bool isValid = Validator.TryValidateObject(newCar, context, validationResults, true);

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
                            if (double.TryParse(Console.ReadLine(), out double enginePower) && enginePower >= 500 && enginePower <= 1500)
                            {
                                newCar.EnginePower = enginePower;

                                var validationResults = new List<ValidationResult>();
                                var context = new ValidationContext(newCar);
                                bool isValid = Validator.TryValidateObject(newCar, context, validationResults, true);

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
                        var newLap = _manager.AddFastestLap(circuit, airTemperature, trackTemperature, lapTime, dateOfRecord, newCar);
                        Console.WriteLine($"New Car Added: {PrintExtentions.PrintF1CarDetails(newCar)}");
                        Console.WriteLine($"New Fastest Lap Added: {PrintExtentions.PrintFastestLapDetails(newLap)}");
                        break;
                    }
                    if (Enum.TryParse(carInput, true, out F1Team existingTeam) && Enum.IsDefined(typeof(F1Team), existingTeam))
                    {
                        var newLap = _manager.AddFastestLap(circuit, airTemperature, trackTemperature, lapTime, dateOfRecord, _manager.GetF1CarsByTeam(existingTeam).FirstOrDefault());
                        Console.WriteLine($"New Fastest Lap Added: {PrintExtentions.PrintFastestLapDetails(newLap)}");
                        break;
                    }
                    Console.WriteLine("No car found with the given name. Please try again.");
                } while (true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}. Please try again.");
            }
        }

    }
}
