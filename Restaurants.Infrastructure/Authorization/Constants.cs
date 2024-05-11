namespace Restaurants.Infrastructure.Authorization;

public static class AppClaimTypes
{
    public const string Nationality = "Nationality";
    public const string DateOfBirth = "DateOfBirth";
}

public static class PolicyName
{
    public const string HasNationality = "HasNationality";
    public const string AtLeast20Years = "AtLeast20Years";
    public static string OwnersWithAtLeast2Restaurants = "OwnersWithAtLeast2Restaurants";
}