using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voyago.Models.Entities;
namespace Voyago.Data.Configurations;
public sealed class PackageInclusionConfiguration : IEntityTypeConfiguration<PackageInclusion>
{
    public void Configure(EntityTypeBuilder<PackageInclusion> b)
    {
        b.ToTable("PackageInclusions"); b.HasKey(x => x.Id); b.Property(x => x.Description).IsRequired().HasMaxLength(500); b.HasIndex(x => new { x.TourPackageId, x.DisplayOrder }); b.HasOne(x => x.TourPackage).WithMany(x => x.Inclusions).HasForeignKey(x => x.TourPackageId).OnDelete(DeleteBehavior.Cascade);
    }
}
