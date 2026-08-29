using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voyago.Models.Entities;
namespace Voyago.Data.Configurations;
public sealed class FlightOfferConfiguration : IEntityTypeConfiguration<FlightOffer>
{
    public void Configure(EntityTypeBuilder<FlightOffer> b)
    {
        b.ToTable("FlightOffers"); b.HasKey(x => x.Id); b.Property(x => x.FlightType).HasConversion<string>().HasMaxLength(40); b.Property(x => x.Airline).IsRequired().HasMaxLength(120); b.Property(x => x.OriginCity).IsRequired().HasMaxLength(120); b.Property(x => x.OriginCode).IsRequired().HasMaxLength(3); b.Property(x => x.DestinationCity).IsRequired().HasMaxLength(120); b.Property(x => x.DestinationCode).IsRequired().HasMaxLength(3); b.Property(x => x.Duration).HasConversion(v => v.Ticks, v => TimeSpan.FromTicks(v)); b.Property(x => x.Aircraft).IsRequired().HasMaxLength(120); b.Property(x => x.CabinClass).IsRequired().HasMaxLength(80); b.Property(x => x.Baggage).IsRequired().HasMaxLength(300); b.Property(x => x.Price).HasPrecision(18, 2); b.HasIndex(x => new { x.OriginCode, x.DestinationCode, x.Published }); b.HasIndex(x => x.FlightType);
    }
}
