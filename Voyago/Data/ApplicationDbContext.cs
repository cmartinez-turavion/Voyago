using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Voyago.Models.Entities;

namespace Voyago.Data;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(user => user.FullName).HasMaxLength(160);
            entity.Property(user => user.MembershipTier).HasConversion<string>().HasMaxLength(32);
            entity.Property(user => user.MilesBalance).HasDefaultValue(0L);
            entity.Property(user => user.PreferredCurrency).HasMaxLength(3).HasDefaultValue("USD");
        });
    }
}
