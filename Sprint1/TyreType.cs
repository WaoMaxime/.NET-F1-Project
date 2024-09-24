namespace Sprint1;

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
        switch (tyreType)
        {
            case TyreType.Soft:
                return "Soft Tyre";
            case TyreType.Medium:
                return "Medium Tyre";
            case TyreType.Hard:
                return "Hard Tyre";
            case TyreType.Inter:
                return "Intermediate Tyre";
            case TyreType.FullWet:
                return "Full Wet Tyre";
            default:
                return tyreType.ToString(); 
        }
    }
}


