namespace Domain;

public enum TyreType
{
    Soft,
    Medium,
    Hard,
    Inter,
    FullWet
}
public static class TyreTypeExtensions
{
    public static string ToFriendlyString(this TyreType tyreType)
    {
        return tyreType switch
        {
            TyreType.Soft => "Soft Tyre",
            TyreType.Medium => "Medium Tyre",
            TyreType.Hard => "Hard Tyre",
            TyreType.Inter => "Intermediate Tyre",
            TyreType.FullWet => "Full Wet Tyre",
            _ => tyreType.ToString()
        };
    }
}


