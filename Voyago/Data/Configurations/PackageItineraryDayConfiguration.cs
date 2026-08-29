using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voyago.Models.Entities;
namespace Voyago.Data.Configurations;
public sealed class PackageItineraryDayConfiguration : IEntityTypeConfiguration<PackageItineraryDay>
{
    public void Configure(EntityTypeBuilder<PackageItineraryDay> b)
    {
        b.ToTable("PackageItineraryDays"); b.HasKey(x => x.Id); b.Property(x => x.Title).IsRequired().HasMaxLength(160); b.Property(x => x.Description).IsRequired().HasMaxLength(2000); b.HasIndex(x => new { x.TourPackageId, x.DayNumber }).IsUnique(); b.HasOne(x => x.TourPackage).WithMany(x => x.ItineraryDays).HasForeignKey(x => x.TourPackageId).OnDelete(DeleteBehavior.Cascade);
    }
}
