using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Voyago.Data;
using Voyago.Data.Seed;
using Voyago.Models.Enums;

namespace Voyago.Tests;

public sealed class DemoDataSeederTests
{
    [Fact]
    public async Task SeedAsync_CreatesRequiredDemonstrationData()
    {
        await using var host = await SqliteTestHost.CreateAsync();
        await SeedAsync(host);

        await using var scope = host.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        Assert.Equal(8, await db.Destinations.CountAsync());
        Assert.Equal(10, await db.TourPackages.CountAsync());
        Assert.Equal(30, await db.PackageItineraryDays.CountAsync());
        Assert.Equal(30, await db.PackageInclusions.CountAsync());
        Assert.Equal(8, await db.Hotels.CountAsync());
        Assert.Equal(16, await db.HotelRoomTypes.CountAsync());
        Assert.Equal(8, await db.FlightOffers.CountAsync());
        Assert.Equal(10, await db.Reviews.CountAsync(x => x.ModerationStatus == ReviewModerationStatus.Approved));
        Assert.All(await db.Hotels.Include(x => x.RoomTypes).ToListAsync(), hotel => Assert.True(hotel.RoomTypes.Count >= 2));
    }

    [Fact]
    public async Task SeedAsync_CanRunTwiceWithoutDuplicates()
    {
        await using var host = await SqliteTestHost.CreateAsync();

        await SeedAsync(host);
        await SeedAsync(host);

        await using var scope = host.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        Assert.Equal(8, await db.Destinations.CountAsync());
        Assert.Equal(10, await db.TourPackages.CountAsync());
        Assert.Equal(16, await db.HotelRoomTypes.CountAsync());
        Assert.Equal(10, await db.Reviews.CountAsync());
    }

    private static async Task SeedAsync(SqliteTestHost host)
    {
        await using var scope = host.Services.CreateAsyncScope();
        var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
        await seeder.SeedAsync(CancellationToken.None);
    }
}
