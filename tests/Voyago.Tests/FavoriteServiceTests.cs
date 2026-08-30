using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Voyago.Data;
using Voyago.Data.Seed;
using Voyago.Models.Entities;
using Voyago.Services.Favorites;

namespace Voyago.Tests;

public sealed class FavoriteServiceTests
{
    [Fact]
    public async Task AddDestinationAsync_AddsOnceAndPreventsDuplicates()
    {
        await using var host = await CreateSeededHostAsync();
        await using var scope = host.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userId = await db.Users.Select(x => x.Id).FirstAsync();
        var service = new FavoriteService(db);

        var first = await service.AddDestinationAsync(userId, 1, CancellationToken.None);
        var second = await service.AddDestinationAsync(userId, 1, CancellationToken.None);

        Assert.Equal(FavoriteOperationResult.Added, first);
        Assert.Equal(FavoriteOperationResult.AlreadyExists, second);
        Assert.Equal(1, await db.Favorites.CountAsync(x => x.UserId == userId && x.DestinationId == 1));
    }

    [Fact]
    public async Task RemoveDestinationAsync_OnlyRemovesFavoriteOwnedByUser()
    {
        await using var host = await CreateSeededHostAsync();
        await using var scope = host.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var firstUserId = await db.Users.Select(x => x.Id).FirstAsync();
        var secondUser = new ApplicationUser
        {
            Id = "second-user",
            UserName = "second@voyago.local",
            NormalizedUserName = "SECOND@VOYAGO.LOCAL",
            Email = "second@voyago.local",
            NormalizedEmail = "SECOND@VOYAGO.LOCAL"
        };
        db.Users.Add(secondUser);
        db.Favorites.Add(new Favorite
        {
            UserId = firstUserId,
            DestinationId = 1,
            CreatedAtUtc = DateTime.UtcNow
        });
        await db.SaveChangesAsync();

        var service = new FavoriteService(db);
        var otherUserResult = await service.RemoveDestinationAsync(secondUser.Id, 1, CancellationToken.None);
        var ownerResult = await service.RemoveDestinationAsync(firstUserId, 1, CancellationToken.None);

        Assert.Equal(FavoriteOperationResult.NotFound, otherUserResult);
        Assert.Equal(FavoriteOperationResult.Removed, ownerResult);
    }

    [Fact]
    public async Task AddDestinationAsync_RejectsMissingOrUnpublishedDestination()
    {
        await using var host = await CreateSeededHostAsync();
        await using var scope = host.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userId = await db.Users.Select(x => x.Id).FirstAsync();
        db.Destinations.Add(new Destination
        {
            Id = 100,
            Name = "Destino oculto",
            Country = "Chile",
            Region = "Pruebas",
            Category = "Interno",
            Tagline = "No publicar",
            Description = "Destino de prueba no publicado.",
            ImageUrl = "/images/test.webp",
            BestSeason = "Todo el año",
            RecommendedDuration = 1,
            Published = false
        });
        await db.SaveChangesAsync();

        var service = new FavoriteService(db);

        Assert.Equal(FavoriteOperationResult.NotFound,
            await service.AddDestinationAsync(userId, 999, CancellationToken.None));
        Assert.Equal(FavoriteOperationResult.NotFound,
            await service.AddDestinationAsync(userId, 100, CancellationToken.None));
    }

    private static async Task<SqliteTestHost> CreateSeededHostAsync()
    {
        var host = await SqliteTestHost.CreateAsync();
        await using var scope = host.Services.CreateAsyncScope();
        await scope.ServiceProvider.GetRequiredService<IDataSeeder>().SeedAsync(CancellationToken.None);
        return host;
    }
}
