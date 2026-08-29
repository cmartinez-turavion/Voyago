using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Voyago.Models.Entities;
namespace Voyago.Data;
public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Destination> Destinations => Set<Destination>();
    public DbSet<TourPackage> TourPackages => Set<TourPackage>();
    public DbSet<PackageItineraryDay> PackageItineraryDays => Set<PackageItineraryDay>();
    public DbSet<PackageInclusion> PackageInclusions => Set<PackageInclusion>();
    public DbSet<Hotel> Hotels => Set<Hotel>();
    public DbSet<HotelRoomType> HotelRoomTypes => Set<HotelRoomType>();
    public DbSet<FlightOffer> FlightOffers => Set<FlightOffer>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Favorite> Favorites => Set<Favorite>();
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(user => user.FullName).HasMaxLength(160);
            entity.Property(user => user.MembershipTier).HasConversion<string>().HasMaxLength(32);
            entity.Property(user => user.MilesBalance).HasDefaultValue(0L);
            entity.Property(user => user.PreferredCurrency).HasMaxLength(3).HasDefaultValue("USD");
        });
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
