using Microsoft.Extensions.DependencyInjection;
using Voyago.Data.Seed;
using Voyago.Pages;
using Voyago.Services.Queries;

namespace Voyago.Tests;

public sealed class HomePageModelTests
{
    [Fact]
    public async Task OnGetAsync_LoadsLimitedRealContent()
    {
        await using var host = await SqliteTestHost.CreateAsync();
        await using var scope = host.Services.CreateAsyncScope();
        await scope.ServiceProvider.GetRequiredService<IDataSeeder>().SeedAsync(CancellationToken.None);
        var db = scope.ServiceProvider.GetRequiredService<Voyago.Data.ApplicationDbContext>();
        var model = new IndexModel(
            new DestinationQueryService(db),
            new PackageQueryService(db),
            new ReviewQueryService(db));

        await model.OnGetAsync(CancellationToken.None);

        Assert.InRange(model.ViewModel.FeaturedDestinations.Count, 1, 4);
        Assert.InRange(model.ViewModel.FeaturedPackages.Count, 1, 3);
        Assert.InRange(model.ViewModel.SelectedReviews.Count, 1, 3);
        Assert.All(model.ViewModel.FeaturedDestinations, item => Assert.True(item.Featured));
    }
}
