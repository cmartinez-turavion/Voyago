using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voyago.Models.Entities;
namespace Voyago.Data.Configurations;
public sealed class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
{
    public void Configure(EntityTypeBuilder<Favorite> b)
    {
        b.ToTable("Favorites"); b.HasKey(x => x.Id); b.HasIndex(x => new { x.UserId, x.DestinationId }).IsUnique(); b.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade); b.HasOne(x => x.Destination).WithMany(x => x.Favorites).HasForeignKey(x => x.DestinationId).OnDelete(DeleteBehavior.Restrict);
    }
}
