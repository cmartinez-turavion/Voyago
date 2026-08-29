namespace Voyago.Models.Entities;
public sealed class PackageItineraryDay { public int Id { get; set; } public int TourPackageId { get; set; } public int DayNumber { get; set; } public string Title { get; set; } = string.Empty; public string Description { get; set; } = string.Empty; public TourPackage TourPackage { get; set; } = null!; }
