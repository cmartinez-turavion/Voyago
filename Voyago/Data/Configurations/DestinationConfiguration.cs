using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voyago.Models.Entities;
namespace Voyago.Data.Configurations;
public sealed class DestinationConfiguration : IEntityTypeConfiguration<Destination>
{
    public void Configure(EntityTypeBuilder<Destination> b)
    {
        b.ToTable("Destinations"); b.HasKey(x => x.Id); b.Property(x => x.Name).IsRequired().HasMaxLength(120); b.Property(x => x.Country).IsRequired().HasMaxLength(80); b.Property(x => x.Region).IsRequired().HasMaxLength(80); b.Property(x => x.Category).IsRequired().HasMaxLength(80); b.Property(x => x.Tagline).IsRequired().HasMaxLength(180); b.Property(x => x.Description).IsRequired().HasMaxLength(4000); b.Property(x => x.ImageUrl).IsRequired().HasMaxLength(500); b.Property(x => x.BestSeason).IsRequired().HasMaxLength(80); b.HasIndex(x => x.Name); b.HasIndex(x => new { x.Country, x.Region }); b.HasIndex(x => new { x.Published, x.Featured });
    }
}
