using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voyago.Models.Entities;
namespace Voyago.Data.Configurations;
public sealed class HotelRoomTypeConfiguration : IEntityTypeConfiguration<HotelRoomType>
{
    public void Configure(EntityTypeBuilder<HotelRoomType> b)
    {
        b.ToTable("HotelRoomTypes"); b.HasKey(x => x.Id); b.Property(x => x.Name).IsRequired().HasMaxLength(120); b.Property(x => x.Size).HasPrecision(8, 2); b.Property(x => x.BedType).IsRequired().HasMaxLength(80); b.Property(x => x.PricePerNight).HasPrecision(18, 2); b.HasIndex(x => x.HotelId); b.HasOne(x => x.Hotel).WithMany(x => x.RoomTypes).HasForeignKey(x => x.HotelId).OnDelete(DeleteBehavior.Cascade);
    }
}
