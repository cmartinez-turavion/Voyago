using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voyago.Models.Entities;
namespace Voyago.Data.Configurations;
public sealed class HotelConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> b)
    {
        b.ToTable("Hotels"); b.HasKey(x => x.Id); b.Property(x => x.Name).IsRequired().HasMaxLength(160); b.Property(x => x.Description).IsRequired().HasMaxLength(4000); b.Property(x => x.Rating).HasPrecision(3, 2); b.Property(x => x.PricePerNight).HasPrecision(18, 2); b.Property(x => x.ImageUrl).IsRequired().HasMaxLength(500); b.HasIndex(x => new { x.DestinationId, x.Published }); b.HasOne(x => x.Destination).WithMany(x => x.Hotels).HasForeignKey(x => x.DestinationId).OnDelete(DeleteBehavior.Restrict);
    }
}
