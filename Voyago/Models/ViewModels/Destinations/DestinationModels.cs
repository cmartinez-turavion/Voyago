namespace Voyago.Models.ViewModels.Destinations;

public sealed record DestinationQueryCriteria(
    string? Search = null,
    string? Region = null,
    string? Category = null,
    string? BestSeason = null,
    int PageNumber = 1,
    int PageSize = 6);

public sealed record DestinationFilterOptionsViewModel(
    IReadOnlyList<string> Regions,
    IReadOnlyList<string> Categories,
    IReadOnlyList<string> BestSeasons);

public sealed record DestinationCardViewModel(
    int Id,
    string Name,
    string Country,
    string Region,
    string Category,
    string Tagline,
    string ImageUrl,
    string BestSeason,
    int RecommendedDuration,
    bool Featured);

public sealed record DestinationRelatedPackageViewModel(
    int Id,
    string Title,
    int DurationDays,
    string TravelStyle,
    decimal PricePerPerson,
    decimal? OriginalPrice,
    string ImageUrl,
    int AvailableSlots,
    bool Featured);

public sealed record DestinationRelatedHotelViewModel(
    int Id,
    string Name,
    int Stars,
    decimal Rating,
    decimal PricePerNight,
    string ImageUrl);

public sealed record DestinationDetailViewModel(
    int Id,
    string Name,
    string Country,
    string Region,
    string Category,
    string Tagline,
    string Description,
    string ImageUrl,
    double Latitude,
    double Longitude,
    string BestSeason,
    int RecommendedDuration,
    bool Featured,
    IReadOnlyList<DestinationRelatedPackageViewModel> Packages,
    IReadOnlyList<DestinationRelatedHotelViewModel> Hotels);
