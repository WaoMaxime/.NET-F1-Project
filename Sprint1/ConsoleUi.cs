namespace Sprint1
{
    public enum F1Team
    {
        Mercedes,
        RedBull,
        Ferrari,
        McLaren,
        AstonMartin,
        Alpine,
        VsCashApp,
        Haas,
        KickSauber,
        Williams
    }

    public class ConsoleUi
    {
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
                        // Get and display all F1 teams
                        var teams = Enum.GetValues(typeof(F1Team));
                        Console.WriteLine("F1 Teams:");
                        foreach (var team in teams)
                        {
                            Console.WriteLine(team);
                        }
                        break;
                    case "2":
                        Console.WriteLine("Which team's fastest laps would you like to show?");
                        break;

                    case "3":
                        ShowTyreTypes();
                        break;

                    case "4":
                        Console.WriteLine("Enter (part of) a TeamName or leave blank:");
                        string? teamName = Console.ReadLine();

                        Console.WriteLine("Enter a TyreType or leave blank:");
                        string? tyreType = Console.ReadLine();
                        Console.WriteLine("Showing fastest lap (Not implemented yet).");
                        break;
                }
            } while (Console.ReadLine() != "0");
        }
        public void ShowTyreTypes()
        {
            var tyreTypes = Enum.GetValues(typeof(TyreType));

            Console.WriteLine("Available Tyre Types:");
            foreach (TyreType tyre in tyreTypes)
            {
                // Use the custom ToFriendlyString method
                Console.WriteLine(tyre.ToFriendlyString());
            }
        }
    }
}