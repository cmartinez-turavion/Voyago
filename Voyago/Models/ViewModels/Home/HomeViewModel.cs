using Voyago.Models.ViewModels.Destinations;

namespace Voyago.Models.ViewModels.Home;

public sealed record HomePackageViewModel(
    int Id,
    int DestinationId,
    string DestinationName,
    string Title,
    int DurationDays,
    string TravelStyle,
    decimal PricePerPerson,
    string ImageUrl,
    int AvailableSlots);

public sealed record HomeReviewViewModel(
    int Id,
    int Rating,
    string Title,
    string Comment,
    bool VerifiedBooking,
    DateTime CreatedAtUtc,
    string? DestinationName,
    string? PackageTitle);

public sealed record HomeViewModel(
    IReadOnlyList<DestinationCardViewModel> FeaturedDestinations,
    IReadOnlyList<HomePackageViewModel> FeaturedPackages,
    IReadOnlyList<HomeReviewViewModel> SelectedReviews);
