using Voyago.Models.Enums;
namespace Voyago.Models.ViewModels.Catalog;
public sealed record DestinationDto(int Id, string Name, string Country, string Region, string Category, string Tagline, string ImageUrl, bool Featured);
public sealed record PackageDto(int Id, int DestinationId, string Title, int DurationDays, string TravelStyle, decimal PricePerPerson, string ImageUrl, int AvailableSlots, bool Featured);
public sealed record HotelDto(int Id, int DestinationId, string Name, int Stars, decimal Rating, decimal PricePerNight, string ImageUrl);
public sealed record FlightDto(int Id, FlightType FlightType, string Airline, string OriginCode, string DestinationCode, TimeSpan Duration, decimal Price, bool CarbonOffsetIncluded);
public sealed record ReviewDto(int Id, int Rating, string Title, string Comment, bool VerifiedBooking, DateTime CreatedAtUtc);
