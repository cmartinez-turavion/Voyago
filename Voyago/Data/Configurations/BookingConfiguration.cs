using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voyago.Models.Entities;
namespace Voyago.Data.Configurations;
public sealed class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> b)
    {
        b.ToTable("Bookings"); b.HasKey(x => x.Id); b.Property(x => x.BookingReference).IsRequired().HasMaxLength(32); b.Property(x => x.ProductType).HasConversion<string>().HasMaxLength(40); b.Property(x => x.ItemTitle).IsRequired().HasMaxLength(200); b.Property(x => x.Destination).IsRequired().HasMaxLength(160); b.Property(x => x.TotalPrice).HasPrecision(18, 2); b.Property(x => x.Currency).IsRequired().HasMaxLength(3); b.Property(x => x.Status).HasConversion<string>().HasMaxLength(24); b.Property(x => x.SpecialRequests).HasMaxLength(2000); b.HasIndex(x => x.BookingReference).IsUnique(); b.HasIndex(x => new { x.UserId, x.Status }); b.HasIndex(x => x.CreatedAtUtc); b.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
    }
}
