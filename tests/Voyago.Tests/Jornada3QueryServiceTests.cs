using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Voyago.Data;
using Voyago.Data.Seed;
using Voyago.Models.Entities;
using Voyago.Models.Enums;
using Voyago.Models.ViewModels.Destinations;
using Voyago.Services.Queries;

namespace Voyago.Tests;

public sealed class Jornada3QueryServiceTests
{
    [Fact]
    public async Task DestinationQuery_AppliesCombinedFiltersAndReturnsDetails()
    {
        await using var host = await CreateSeededHostAsync();
        await using var scope = host.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var service = new DestinationQueryService(db);

        var page = await service.GetPageAsync(
            new DestinationQueryCriteria(
                Search: "Chile",
                Region: "Magallanes",
                Category: "Aventura y naturaleza",
                BestSeason: "Noviembre a marzo",
                PageNumber: 1,
                PageSize: 6),
            CancellationToken.None);

        var detail = await service.GetByIdAsync(6, CancellationToken.None);

        Assert.Single(page.Items);
        Assert.Equal("Patagonia", page.Items[0].Name);
        Assert.NotNull(detail);
        Assert.Equal("Patagonia", detail.Name);
        Assert.NotEmpty(detail.Packages);
        Assert.NotEmpty(detail.Hotels);
    }

    [Fact]
    public async Task DestinationQuery_DoesNotReturnUnpublishedDetailsOrFilterOptions()
    {
        await using var host = await CreateSeededHostAsync();
        await using var scope = host.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Destinations.Add(new Destination
        {
            Id = 100,
            Name = "Oculto",
            Country = "Chile",
            Region = "Región oculta",
            Category = "Categoría oculta",
            Tagline = "No publicar",
            Description = "Destino no publicado.",
            ImageUrl = "/images/test.webp",
            BestSeason = "Temporada oculta",
            RecommendedDuration = 1,
            Published = false
        });
        await db.SaveChangesAsync();
        var service = new DestinationQueryService(db);

        var detail = await service.GetByIdAsync(100, CancellationToken.None);
        var options = await service.GetFilterOptionsAsync(CancellationToken.None);

        Assert.Null(detail);
        Assert.DoesNotContain("Región oculta", options.Regions);
        Assert.DoesNotContain("Categoría oculta", options.Categories);
        Assert.DoesNotContain("Temporada oculta", options.BestSeasons);
    }

    [Fact]
    public async Task HomeQueries_ReturnLimitedPublishedAndApprovedContent()
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
            Rating = 1,
            Title = "Pendiente",
            Comment = "No debe aparecer en Home.",
            ModerationStatus = ReviewModerationStatus.Pending,
            CreatedAtUtc = DateTime.UtcNow
        });
        await db.SaveChangesAsync();

        var destinations = await new DestinationQueryService(db)
            .GetFeaturedAsync(4, CancellationToken.None);
        var packages = await new PackageQueryService(db)
            .GetFeaturedAsync(3, CancellationToken.None);
        var reviews = await new ReviewQueryService(db)
            .GetSelectedApprovedAsync(3, CancellationToken.None);

        Assert.InRange(destinations.Count, 1, 4);
        Assert.InRange(packages.Count, 1, 3);
        Assert.InRange(reviews.Count, 1, 3);
        Assert.DoesNotContain(reviews, x => x.Id == 100);
    }

    private static async Task<SqliteTestHost> CreateSeededHostAsync()
    {
        var host = await SqliteTestHost.CreateAsync();
        await using var scope = host.Services.CreateAsyncScope();
        await scope.ServiceProvider.GetRequiredService<IDataSeeder>().SeedAsync(CancellationToken.None);
        return host;
    }
}
