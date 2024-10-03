
using Domain;

namespace CA
{

    public class ConsoleUi
    {
        private List<F1Car> _cars;
        private List<FastestLap> _laps;

        public ConsoleUi()
        {
            Seed();
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
                                  "\n3) Show all tyretypes" +
                                  "\n4) Show fastest lap with team and/or tyretype" +
                                  "\nChoice (0-4):");

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
                }
            } while (Console.ReadLine() != "0");
        }

        private void Seed()
        {
            _cars = new List<F1Car>
            {
                new F1Car(F1Team.Mercedes, "W18", 4, 6.5, DateTime.Parse("2024-03-14"),TyreType.Soft, 1025),
                new F1Car(F1Team.RedBull, "RB16", 2, 1.3, DateTime.Parse("2024-04-12"),TyreType.Medium, 1000),
                new F1Car(F1Team.Ferrari, "SF24", 3, 7.8, DateTime.Parse("2024-05-21"), TyreType.Soft),
                new F1Car(F1Team.McLaren, "MCL35", 1, 2.4, DateTime.Parse("2024-06-22"),TyreType.Hard),
                new F1Car(F1Team.AstonMartin, "AS15", 5, 9.10, DateTime.Parse("2024-02-12"),TyreType.Soft,980),
                new F1Car(F1Team.Alpine, "ALP12", 6, 15.17, DateTime.Parse("2024-03-16"),TyreType.Soft,965),
                new F1Car(F1Team.VsCashApp, "VSC5", 10, 12.16, DateTime.Parse("2024-02-18"),TyreType.Hard,974),
                new F1Car(F1Team.Haas, "HS50", 7, 11.14, DateTime.Parse("2024-03-25"),TyreType.Hard,963),
                new F1Car(F1Team.KickSauber, "KS5", 8, 19.20, DateTime.Parse("2024-02-23"),TyreType.Medium,896),
                new F1Car(F1Team.Williams, "WW52", 9, 13.18, DateTime.Parse("2024-03-20"),TyreType.Hard,950)
            };

            _laps = new List<FastestLap>
            {
                new FastestLap("Monza", 28, 40, new TimeSpan(0, 0, 1, 19,327), DateTime.Parse("2024-09-01"), _cars[2]),
                new FastestLap("Silverstone", 26, 38, new TimeSpan(0, 0, 1, 25,819), DateTime.Parse("2024-07-07"), _cars[0]),
                new FastestLap("Spa", 23, 36, new TimeSpan(0, 0, 1, 42,847), DateTime.Parse("2024-08-28"), _cars[1]),
                new FastestLap("Singapore", 21, 34, new TimeSpan(0, 0, 1, 29,525), DateTime.Parse("2024-09-22"), _cars[3]),
                new FastestLap("Texas", 24, 34, new TimeSpan(0, 0, 1, 51,362), DateTime.Parse("2024-06-12"), _cars[4]),
                new FastestLap("France", 30, 34, new TimeSpan(0, 0, 1, 36,241), DateTime.Parse("2024-05-17"), _cars[9]),
                new FastestLap("Hungary", 21, 34, new TimeSpan(0, 0, 1, 24,552), DateTime.Parse("2024-05-01"), _cars[5]),
                new FastestLap("Netherlands", 24, 34, new TimeSpan(0, 0, 1, 35,522), DateTime.Parse("2024-06-25"), _cars[7]),
                new FastestLap("Spain", 26, 34, new TimeSpan(0, 0, 1, 42,624), DateTime.Parse("2024-08-04"), _cars[6]),
                new FastestLap("Australia", 27, 34, new TimeSpan(0, 0, 1, 21,155), DateTime.Parse("2024-08-09"), _cars[8])
            };
        }

        private void ShowAllTeams()
        {
            foreach (var car in _cars)
            {
                Console.WriteLine(car.ToString());
            }
        }

        private void ShowFastestLapsByTeam()
        {
            Console.WriteLine("Enter (part of) a TeamName");
            string teamName = Console.ReadLine();

            if (teamName!= "")
            {
                foreach (var lap in _laps)
                {
                    // Check if the input teamName is a substring of the team's name (case-insensitive)
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
            string? driverPositionInput = Console.ReadLine();
            double? driverPosition = null;
            
            if (!string.IsNullOrEmpty(driverPositionInput))
            {
                if (double.TryParse(driverPositionInput, out double parsedPosition))
                {
                    driverPosition = parsedPosition;
                }
            }

            Console.WriteLine("Enter a Lap Time (as a number in seconds, e.g., 85.327) or leave blank:");
            string? lapTimeInput = Console.ReadLine();
            TimeSpan? lapTime = null;
            
            if (!string.IsNullOrEmpty(lapTimeInput))
            {
                if (double.TryParse(lapTimeInput, out double parsedLapTimeInSeconds))
                {
                    lapTime = TimeSpan.FromSeconds(parsedLapTimeInSeconds);
                }
            }

            foreach (var lap in _laps)
            {
                bool driverPositionMatch = !driverPosition.HasValue || lap.Car.DriversPositions == driverPosition.Value;
                bool lapTimeMatch = !lapTime.HasValue || lap.LapTime.TotalSeconds == lapTime.Value.TotalSeconds;

                if (driverPositionMatch && lapTimeMatch)
                {
                    Console.WriteLine(lap.ToString());
                }
            }
        }

    }
}