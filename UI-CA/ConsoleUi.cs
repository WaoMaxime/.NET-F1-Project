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
                                  "\n1) Show all teams" +
                                  "\n2) Show fastest laps of team" +
                                  "\n3) Show all tyre types" +
                                  "\n4) Show fastest lap with team and/or tyre type" +
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
            foreach (var car in cars)
            {
                Console.WriteLine(PrintExtentions.PrintF1CarDetails(car));
            }
        }

        private void ShowFastestLapsByTeam()
        {
            Console.WriteLine("Enter (part of) a Team Name:");
            string teamName = Console.ReadLine();

            if (!string.IsNullOrEmpty(teamName))
            {
                var laps = _manager.GetAllFastestLaps();
                foreach (var lap in laps)
                {
                    if (lap.Car.Team.ToString().IndexOf(teamName, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        Console.WriteLine(PrintExtentions.PrintFastestLapDetails(lap));
                    }
                }
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
            try
            {
                Console.WriteLine("Enter the Team's Constructors's positions in the championship (as a number) or leave blank:");
                string driverPositionInput = Console.ReadLine();
                double? driverPosition = null;

                if (!string.IsNullOrEmpty(driverPositionInput))
                {
                    if (double.TryParse(driverPositionInput, out double parsedPosition))
                    {
                        driverPosition = parsedPosition;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input for Constructors's Position. Please enter a valid number.");
                        return; 
                    }
                }

                Console.WriteLine("Enter a Lap Time (in the format 'minutes.seconds.milliseconds', e.g., 1.40.800) or leave blank:");
                string lapTimeInput = Console.ReadLine();
                TimeSpan? lapTime = null;

                if (!string.IsNullOrEmpty(lapTimeInput))
                {
                    string[] parts = lapTimeInput.Split('.');
                    if (parts.Length == 3 &&
                        int.TryParse(parts[0], out int minutes) &&
                        int.TryParse(parts[1], out int seconds) &&
                        int.TryParse(parts[2], out int milliseconds))
                    {
                        lapTime = new TimeSpan(0, 0, minutes, seconds, milliseconds);
                    }
                    else
                    {
                        Console.WriteLine("Invalid lap time format. Please use 'minutes.seconds.milliseconds'.");
                        return;
                    }
                }

                var laps = _manager.GetAllFastestLaps();
                
                if (laps == null || !laps.Any())
                {
                    Console.WriteLine("No fastest laps found.");
                    return;
                }

                foreach (var lap in laps)
                {
                    if (lap.Car == null)
                    {
                        Console.WriteLine("Error: A lap is missing associated car data.");
                        continue;
                    }

                    bool driverPositionMatch = !driverPosition.HasValue || lap.Car.DriversPositions == driverPosition.Value;
                    bool lapTimeMatch = !lapTime.HasValue || Math.Abs((lap.LapTime - lapTime.Value).TotalMilliseconds) < 10;  // Allow slight differences

                    if (driverPositionMatch && lapTimeMatch)
                    {
                        Console.WriteLine(PrintExtentions.PrintFastestLapDetails(lap));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}. Please try again.");
            }
        }




        private void AddNewF1Car()
        {
            try
            {
                Console.WriteLine("Enter Team (e.g., Mercedes, RedBull, Ferrari):");
                var team = (F1Team)Enum.Parse(typeof(F1Team), Console.ReadLine());

                Console.WriteLine("Enter Chasis:");
                string chasis = Console.ReadLine();

                Console.WriteLine("Enter Constructors Position:");
                int constructorsPosition = int.Parse(Console.ReadLine());

                Console.WriteLine("Enter Driver Positions:");
                double driverPositions = double.Parse(Console.ReadLine());

                Console.WriteLine("Enter Manufacture Date (yyyy-mm-dd):");
                DateTime manufactureDate = DateTime.Parse(Console.ReadLine());

                Console.WriteLine("Enter Tyre Type (Soft, Medium, Hard, Inter, FullWet):");
                var tyreType = (TyreType)Enum.Parse(typeof(TyreType), Console.ReadLine());

                Console.WriteLine("Enter Engine Power (HP) (or leave blank):");
                string enginePowerInput = Console.ReadLine();
                double? enginePower = string.IsNullOrEmpty(enginePowerInput) ? (double?)null : double.Parse(enginePowerInput);

                var newCar = _manager.AddF1Car(team, chasis, constructorsPosition, driverPositions, manufactureDate, tyreType, enginePower);
                Console.WriteLine($"New F1 Car Added: {PrintExtentions.PrintF1CarDetails(newCar)}");
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
                string circuit;
                do
                {
                    Console.WriteLine("Enter Circuit Name (min 3 characters, max 50):");
                    circuit = Console.ReadLine();
                } while (string.IsNullOrWhiteSpace(circuit) || circuit.Length < 3 || circuit.Length > 50);
                
                int airTemperature;
                do
                {
                    Console.WriteLine("Enter Air Temperature (between -30 and 50 degrees Celsius):");
                } while (!int.TryParse(Console.ReadLine(), out airTemperature) || airTemperature < -30 || airTemperature > 50);
                
                int trackTemperature;
                do
                {
                    Console.WriteLine("Enter Track Temperature (between -30 and 70 degrees Celsius):");
                } while (!int.TryParse(Console.ReadLine(), out trackTemperature) || trackTemperature < -30 || trackTemperature > 70);
                
                TimeSpan lapTime;
                do
                {
                    Console.WriteLine("Enter Lap Time (in the format 'minutes.seconds.milliseconds', e.g., 1.40.564):");
                    string lapTimeInput = Console.ReadLine();
                    string[] parts = lapTimeInput.Split('.');
                    if (parts.Length == 3 &&
                        int.TryParse(parts[0], out int minutes) &&
                        int.TryParse(parts[1], out int seconds) &&
                        int.TryParse(parts[2], out int milliseconds))
                    {
                        lapTime = new TimeSpan(0, 0, minutes, seconds, milliseconds);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid format. Please enter the lap time in 'minutes.seconds.milliseconds'.");
                    }
                } while (true);
                
                DateTime dateOfRecord;
                do
                {
                    Console.WriteLine("Enter Date of Record (yyyy-mm-dd):");
                } while (!DateTime.TryParse(Console.ReadLine(), out dateOfRecord) || dateOfRecord > DateTime.Now);
                
                F1Car car = null;
                do
                {
                    Console.WriteLine("Enter the ID of the car used (or type 'new' to add a new car):");
                    string carInput = Console.ReadLine();

                    if (carInput.ToLower() == "new")
                    {
                        F1Team team;
                        do
                        {
                            Console.WriteLine("Enter Team (e.g., RedBull, Mercedes, etc.):");
                        } while (!Enum.TryParse<F1Team>(Console.ReadLine(), out team));

                        Console.WriteLine("Enter Chassis Name:");
                        string chassis = Console.ReadLine();

                        int constructorsPosition;
                        do
                        {
                            Console.WriteLine("Enter Constructors Position (1-10):");
                        } while (!int.TryParse(Console.ReadLine(), out constructorsPosition) || constructorsPosition < 1 || constructorsPosition > 10);

                        double driversPosition;
                        do
                        {
                            Console.WriteLine("Enter Driver's Position in Championship (1-20):");
                        } while (!double.TryParse(Console.ReadLine(), out driversPosition) || driversPosition < 1 || driversPosition > 20);

                        DateTime manufactureDate;
                        do
                        {
                            Console.WriteLine("Enter Manufacture Date (yyyy-mm-dd):");
                        } while (!DateTime.TryParse(Console.ReadLine(), out manufactureDate) || manufactureDate > DateTime.Now);

                        TyreType tyres;
                        do
                        {
                            Console.WriteLine("Enter Tyre Type (Soft, Medium, Hard):");
                        } while (!Enum.TryParse<TyreType>(Console.ReadLine(), out tyres));

                        double enginePower;
                        do
                        {
                            Console.WriteLine("Enter Engine Power (in HP, e.g., 1000):");
                        } while (!double.TryParse(Console.ReadLine(), out enginePower) || enginePower < 500 || enginePower > 1500);

                        // Create a new car
                        car = new F1Car(team, chassis, constructorsPosition, driversPosition, manufactureDate, tyres, enginePower);
                        break;
                    }
                    else if (int.TryParse(carInput, out int carId))
                    {
                        car = _manager.GetF1Car(carId);
                        if (car != null)
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("No car found with the given ID. Please try again.");
                        }
                    }
                } while (true);
                
                var newLap = _manager.AddFastestLap(circuit, airTemperature, trackTemperature, lapTime, dateOfRecord, car);
                
                Console.WriteLine($"New Fastest Lap Added: {PrintExtentions.PrintFastestLapDetails(newLap)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}. Please try again.");
            }
        }

    }
}
