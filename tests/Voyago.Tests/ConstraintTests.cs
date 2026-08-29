using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Voyago.Data;
using Voyago.Data.Seed;
using Voyago.Models.Entities;
using Voyago.Models.Enums;

namespace Voyago.Tests;

public sealed class ConstraintTests
{
    [Fact]
    public async Task BookingReference_MustBeUnique_AndFinancialValuesRoundTrip()
    {
        await using var host = await CreateSeededHostAsync();
        await using var scope = host.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userId = await db.Users.Select(x => x.Id).FirstAsync();
        var createdAt = new DateTime(2026, 8, 29, 14, 0, 0, DateTimeKind.Utc);

        db.Bookings.Add(CreateBooking(userId, "VY-TEST-001", 12345.67m, createdAt));
        await db.SaveChangesAsync();
        db.ChangeTracker.Clear();

        var persisted = await db.Bookings.SingleAsync(x => x.BookingReference == "VY-TEST-001");
        Assert.Equal(12345.67m, persisted.TotalPrice);
        Assert.Equal(createdAt, persisted.CreatedAtUtc);

        db.Bookings.Add(CreateBooking(userId, "VY-TEST-001", 1m, createdAt));
        await Assert.ThrowsAsync<DbUpdateException>(() => db.SaveChangesAsync());
    }

    [Fact]
    public async Task Favorite_UserAndDestinationCombinationMustBeUnique()
    {
        await using var host = await CreateSeededHostAsync();
        await using var scope = host.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userId = await db.Users.Select(x => x.Id).FirstAsync();

        db.Favorites.Add(new Favorite { UserId = userId, DestinationId = 1, CreatedAtUtc = DateTime.UtcNow });
        await db.SaveChangesAsync();
        db.Favorites.Add(new Favorite { UserId = userId, DestinationId = 1, CreatedAtUtc = DateTime.UtcNow });

        await Assert.ThrowsAsync<DbUpdateException>(() => db.SaveChangesAsync());
    }

    private static Booking CreateBooking(string userId, string reference, decimal total, DateTime createdAt) => new()
    {
        BookingReference = reference,
        UserId = userId,
        ProductType = BookingProductType.TourPackage,
        ProductId = 1,
        ItemTitle = "Voyago Signature Journey 1",
        Destination = "Costa Amalfitana",
        StartDate = createdAt.AddDays(30),
        EndDate = createdAt.AddDays(37),
        GuestsCount = 2,
        TotalPrice = total,
        Currency = "USD",
        Status = BookingStatus.Draft,
        CreatedAtUtc = createdAt,
        UpdatedAtUtc = createdAt
    };

    private static async Task<SqliteTestHost> CreateSeededHostAsync()
    {
        var host = await SqliteTestHost.CreateAsync();
        await using var scope = host.Services.CreateAsyncScope();
        await scope.ServiceProvider.GetRequiredService<IDataSeeder>().SeedAsync(CancellationToken.None);
        return host;
    }
}
