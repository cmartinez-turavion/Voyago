namespace Voyago.Models.Entities;
public sealed class Destination
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Tagline { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string BestSeason { get; set; } = string.Empty;
    public int RecommendedDuration { get; set; }
    public bool Featured { get; set; }
    public bool Published { get; set; }
    public ICollection<TourPackage> TourPackages { get; set; } = new List<TourPackage>();
    public ICollection<Hotel> Hotels { get; set; } = new List<Hotel>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
}
