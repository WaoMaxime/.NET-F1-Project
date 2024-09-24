namespace Sprint1;

public class F1Car
{
    public F1Team Team { get; set; }
    public string Chasis { get; set; }
    public int ConstructorsPosition { get; set; }
    public double DriversPositions { get; set; }

    public F1Car(F1Team team, string chasis, int constructorsPosition, double driversPositions)
    {
        Team = team;
        Chasis = chasis;
        ConstructorsPosition = constructorsPosition;
        DriversPositions = driversPositions;
    }

    public override string ToString()
    {
        return "Het Team {0} met chasis {1} met constructorsposition {2}, heeft in driverspostitions de plekken {3}";
    }
}

