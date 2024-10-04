using BusinessLayer;
using Domain;

namespace UI_CA
{
    public class ConsoleUi
    {
        private readonly IManager _manager;

        // Updated constructor to accept IManager
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
                Console.WriteLine(car.ToString());
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
                        Console.WriteLine(lap.ToString());
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
            Console.WriteLine("Enter the Team's Driver's positions in the championship (as a number) or leave blank:");
            string driverPositionInput = Console.ReadLine();
            double? driverPosition = null;

            if (!string.IsNullOrEmpty(driverPositionInput))
            {
                if (double.TryParse(driverPositionInput, out double parsedPosition))
                {
                    driverPosition = parsedPosition;
                }
            }

            Console.WriteLine("Enter a Lap Time (as a number in seconds, e.g., 85.327) or leave blank:");
            string lapTimeInput = Console.ReadLine();
            TimeSpan? lapTime = null;

            if (!string.IsNullOrEmpty(lapTimeInput))
            {
                if (double.TryParse(lapTimeInput, out double parsedLapTimeInSeconds))
                {
                    lapTime = TimeSpan.FromSeconds(parsedLapTimeInSeconds);
                }
            }

            var laps = _manager.GetAllFastestLaps();
            foreach (var lap in laps)
            {
                bool driverPositionMatch = !driverPosition.HasValue || lap.Car.DriversPositions == driverPosition.Value;
                bool lapTimeMatch = !lapTime.HasValue || Math.Abs(lap.LapTime.TotalSeconds - lapTime.Value.TotalSeconds) < 0.001;

                if (driverPositionMatch && lapTimeMatch)
                {
                    Console.WriteLine(lap.ToString());
                }
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
                Console.WriteLine($"New F1 Car Added: {newCar}");
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
                Console.WriteLine("Enter Circuit Name:");
                string circuit = Console.ReadLine();

                Console.WriteLine("Enter Air Temperature:");
                int airTemperature = int.Parse(Console.ReadLine());

                Console.WriteLine("Enter Track Temperature:");
                int trackTemperature = int.Parse(Console.ReadLine());

                Console.WriteLine("Enter Lap Time (in seconds, e.g., 85.327):");
                double lapTimeInSeconds = double.Parse(Console.ReadLine());
                TimeSpan lapTime = TimeSpan.FromSeconds(lapTimeInSeconds);

                Console.WriteLine("Enter Date of Record (yyyy-mm-dd):");
                DateTime dateOfRecord = DateTime.Parse(Console.ReadLine());

                Console.WriteLine("Enter the ID of the car used:");
                int carId = int.Parse(Console.ReadLine());
                F1Car car = _manager.GetF1Car(carId);

                var newLap = _manager.AddFastestLap(circuit, airTemperature, trackTemperature, lapTime, dateOfRecord, car);
                Console.WriteLine($"New Fastest Lap Added: {newLap}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}. Please try again.");
            }
        }
    }
}
