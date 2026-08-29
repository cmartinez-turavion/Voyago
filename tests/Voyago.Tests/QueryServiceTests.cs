using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Voyago.Data;
using Voyago.Data.Seed;
using Voyago.Models.Entities;
using Voyago.Models.Enums;
using Voyago.Services.Queries;

namespace Voyago.Tests;

public sealed class QueryServiceTests
{
    [Fact]
    public async Task DestinationQuery_ReturnsOnlyPublishedAndPaginates()
    {
        await using var host = await CreateSeededHostAsync();
        await using var scope = host.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Destinations.Add(new Destination
        {
            Id = 100,
            Name = "Destino oculto",
            Country = "Chile",
            Region = "Pruebas",
            Category = "Interno",
            Tagline = "No publicar",
            Description = "Registro para comprobar el filtro de publicación.",
            ImageUrl = "/images/test.webp",
            BestSeason = "Todo el año",
            RecommendedDuration = 1,
            Published = false
        });
        await db.SaveChangesAsync();

        var service = new DestinationQueryService(db);
        var first = await service.GetPageAsync(1, 3, CancellationToken.None);
        var second = await service.GetPageAsync(2, 3, CancellationToken.None);

        Assert.Equal(8, first.TotalItems);
        Assert.Equal(3, first.Items.Count);
        Assert.Equal(3, second.Items.Count);
        Assert.Empty(first.Items.Select(x => x.Id).Intersect(second.Items.Select(x => x.Id)));
        Assert.DoesNotContain(first.Items.Concat(second.Items), x => x.Id == 100);
    }

    [Fact]
    public async Task ReviewQuery_ReturnsOnlyApprovedReviews()
    {
        await using var host = await CreateSeededHostAsync();
        await using var scope = host.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userId = await db.Users.Select(x => x.Id).FirstAsync();
        db.Reviews.Add(new Review
        {
            Id = 100,
            UserId = userId,
            DestinationId = 1,
            Rating = 3,
            Title = "Pendiente",
            Comment = "Reseña pendiente de moderación.",
            ModerationStatus = ReviewModerationStatus.Pending,
            CreatedAtUtc = DateTime.UtcNow
        });
        await db.SaveChangesAsync();

        var result = await new ReviewQueryService(db).GetPageAsync(1, 50, CancellationToken.None);

        Assert.Equal(10, result.TotalItems);
        Assert.DoesNotContain(result.Items, x => x.Id == 100);
    }

    [Fact]
    public async Task AllCatalogServices_ReturnData()
    {
        await using var host = await CreateSeededHostAsync();
        await using var scope = host.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        Assert.NotEmpty((await new PackageQueryService(db).GetPageAsync(1, 10, CancellationToken.None)).Items);
        Assert.NotEmpty((await new HotelQueryService(db).GetPageAsync(1, 10, CancellationToken.None)).Items);
        Assert.NotEmpty((await new FlightQueryService(db).GetPageAsync(1, 10, CancellationToken.None)).Items);
    }

    private static async Task<SqliteTestHost> CreateSeededHostAsync()
    {
        var host = await SqliteTestHost.CreateAsync();
        await using var scope = host.Services.CreateAsyncScope();
        await scope.ServiceProvider.GetRequiredService<IDataSeeder>().SeedAsync(CancellationToken.None);
        return host;
    }
}
