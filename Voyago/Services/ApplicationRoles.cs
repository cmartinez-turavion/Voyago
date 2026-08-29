namespace Voyago.Services;

public static class ApplicationRoles
{
    public const string Traveler = "Traveler";
    public const string Administrator = "Administrator";

    public static IReadOnlyCollection<string> All { get; } =
        [Traveler, Administrator];
}
