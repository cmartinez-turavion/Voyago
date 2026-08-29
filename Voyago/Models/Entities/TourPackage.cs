namespace Voyago.Models.Entities;
public sealed class TourPackage
{
    public int Id { get; set; }
    public int DestinationId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int DurationDays { get; set; }
    public int GroupSizeMax { get; set; }
    public string TravelStyle { get; set; } = string.Empty;
    public decimal PricePerPerson { get; set; }
    public decimal? OriginalPrice { get; set; }
    public string Overview { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public int AvailableSlots { get; set; }
    public bool Featured { get; set; }
    public bool Published { get; set; }
    public Destination Destination { get; set; } = null!;
    public ICollection<PackageItineraryDay> ItineraryDays { get; set; } = new List<PackageItineraryDay>();
    public ICollection<PackageInclusion> Inclusions { get; set; } = new List<PackageInclusion>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
