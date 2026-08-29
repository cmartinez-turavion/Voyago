using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Voyago.Models.Entities;
using Voyago.Models.Enums;
namespace Voyago.Data.Seed;
public sealed class DemoDataSeeder(ApplicationDbContext db, UserManager<ApplicationUser> users, ILogger<DemoDataSeeder> logger) : IDataSeeder
{
    private const string DemoEmail = "demo-reviewer@voyago.local";
    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        var destinations = DemoData.Destinations();
        var existingIds = await db.Destinations.AsNoTracking().Select(x => x.Id).ToListAsync(cancellationToken);
        await db.Destinations.AddRangeAsync(destinations.Where(x => !existingIds.Contains(x.Id)), cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        await AddMissingAsync(db.TourPackages, DemoData.Packages(), cancellationToken);
        await AddMissingAsync(db.PackageItineraryDays, DemoData.ItineraryDays(), cancellationToken);
        await AddMissingAsync(db.PackageInclusions, DemoData.Inclusions(), cancellationToken);
        await AddMissingAsync(db.Hotels, DemoData.Hotels(), cancellationToken);
        await AddMissingAsync(db.HotelRoomTypes, DemoData.RoomTypes(), cancellationToken);
        await AddMissingAsync(db.FlightOffers, DemoData.Flights(), cancellationToken);

        var user = await users.FindByEmailAsync(DemoEmail);
        if (user is null)
        {
            user = new ApplicationUser { UserName = DemoEmail, Email = DemoEmail, FullName = "Voyago Demo Reviewer", EmailConfirmed = false };
            var result = await users.CreateAsync(user);
            if (!result.Succeeded) throw new InvalidOperationException(string.Join(", ", result.Errors.Select(x => x.Description)));
        }
        var reviewIds = await db.Reviews.AsNoTracking().Select(x => x.Id).ToListAsync(cancellationToken);
        await db.Reviews.AddRangeAsync(DemoData.Reviews(user.Id).Where(x => !reviewIds.Contains(x.Id)), cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Voyago demo data seed completed.");
    }
    private async Task AddMissingAsync<T>(DbSet<T> set, IReadOnlyCollection<T> items, CancellationToken token) where T : class
    {
        var id = db.Model.FindEntityType(typeof(T))!.FindPrimaryKey()!.Properties.Single();
        var existing = await set.AsNoTracking().Select(x => EF.Property<int>(x, id.Name)).ToListAsync(token);
        await set.AddRangeAsync(items.Where(x => !existing.Contains((int)id.PropertyInfo!.GetValue(x)!)), token);
        await db.SaveChangesAsync(token);
    }
}
