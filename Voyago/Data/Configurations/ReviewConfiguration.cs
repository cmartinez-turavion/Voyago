using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voyago.Models.Entities;
namespace Voyago.Data.Configurations;
public sealed class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> b)
    {
        b.ToTable("Reviews"); b.HasKey(x => x.Id); b.Property(x => x.Title).IsRequired().HasMaxLength(160); b.Property(x => x.Comment).IsRequired().HasMaxLength(3000); b.Property(x => x.ModerationStatus).HasConversion<string>().HasMaxLength(24); b.HasIndex(x => new { x.ModerationStatus, x.CreatedAtUtc }); b.HasIndex(x => x.DestinationId); b.HasIndex(x => x.TourPackageId); b.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict); b.HasOne(x => x.Destination).WithMany(x => x.Reviews).HasForeignKey(x => x.DestinationId).OnDelete(DeleteBehavior.Restrict); b.HasOne(x => x.TourPackage).WithMany(x => x.Reviews).HasForeignKey(x => x.TourPackageId).OnDelete(DeleteBehavior.Restrict);
    }
}
