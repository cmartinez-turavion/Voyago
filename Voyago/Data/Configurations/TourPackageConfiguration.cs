using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voyago.Models.Entities;
namespace Voyago.Data.Configurations;
public sealed class TourPackageConfiguration : IEntityTypeConfiguration<TourPackage>
{
    public void Configure(EntityTypeBuilder<TourPackage> b)
    {
        b.ToTable("TourPackages"); b.HasKey(x => x.Id); b.Property(x => x.Title).IsRequired().HasMaxLength(160); b.Property(x => x.TravelStyle).IsRequired().HasMaxLength(80); b.Property(x => x.PricePerPerson).HasPrecision(18, 2); b.Property(x => x.OriginalPrice).HasPrecision(18, 2); b.Property(x => x.Overview).IsRequired().HasMaxLength(4000); b.Property(x => x.ImageUrl).IsRequired().HasMaxLength(500); b.HasIndex(x => new { x.DestinationId, x.Published }); b.HasIndex(x => new { x.Published, x.Featured }); b.HasOne(x => x.Destination).WithMany(x => x.TourPackages).HasForeignKey(x => x.DestinationId).OnDelete(DeleteBehavior.Restrict);
    }
}
