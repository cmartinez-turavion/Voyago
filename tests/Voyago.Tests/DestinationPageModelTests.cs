using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Voyago.Data.Seed;
using Voyago.Models.Entities;
using Voyago.Pages.Destinations;
using Voyago.Services.Favorites;
using Voyago.Services.Queries;

namespace Voyago.Tests;

public sealed class DestinationPageModelTests
{
    [Fact]
    public async Task Details_OnGetAsync_ReturnsNotFoundForMissingDestination()
    {
        await using var host = await CreateSeededHostAsync();
        await using var scope = host.Services.CreateAsyncScope();
        var model = CreateDetailsModel(scope.ServiceProvider);

        var result = await model.OnGetAsync(999, CancellationToken.None);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Details_OnGetAsync_ReturnsPageForPublishedDestination()
    {
        await using var host = await CreateSeededHostAsync();
        await using var scope = host.Services.CreateAsyncScope();
        var model = CreateDetailsModel(scope.ServiceProvider);

        var result = await model.OnGetAsync(1, CancellationToken.None);

        Assert.IsType<PageResult>(result);
        Assert.Equal(1, model.Destination.Id);
    }

    private static DetailsModel CreateDetailsModel(IServiceProvider services)
    {
        var db = services.GetRequiredService<Voyago.Data.ApplicationDbContext>();
        var model = new DetailsModel(
            new DestinationQueryService(db),
            new FavoriteService(db),
            services.GetRequiredService<UserManager<ApplicationUser>>());
        model.PageContext = new PageContext
        {
            HttpContext = new DefaultHttpContext { RequestServices = services }
        };
        return model;
    }

    private static async Task<SqliteTestHost> CreateSeededHostAsync()
    {
        var host = await SqliteTestHost.CreateAsync();
        await using var scope = host.Services.CreateAsyncScope();
        await scope.ServiceProvider.GetRequiredService<IDataSeeder>().SeedAsync(CancellationToken.None);
        return host;
    }
}
