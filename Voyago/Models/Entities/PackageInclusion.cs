namespace Voyago.Models.Entities;
public sealed class PackageInclusion { public int Id { get; set; } public int TourPackageId { get; set; } public string Description { get; set; } = string.Empty; public bool IsIncluded { get; set; } public int DisplayOrder { get; set; } public TourPackage TourPackage { get; set; } = null!; }
