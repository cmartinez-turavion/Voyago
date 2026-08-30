using Microsoft.Extensions.DependencyInjection;
using Voyago.Data.Seed;
using Voyago.Models.ViewModels.Destinations;
using Voyago.Services.Queries;

namespace Voyago.Tests;

public sealed class DestinationSearchRegressionTests
{
    [Fact]
    public async Task Search_IsCaseInsensitiveForPatagonia()
    {
        await using var host = await SqliteTestHost.CreateAsync();
        await using var scope = host.Services.CreateAsyncScope();
        await scope.ServiceProvider.GetRequiredService<IDataSeeder>().SeedAsync(CancellationToken.None);
        var db = scope.ServiceProvider.GetRequiredService<Voyago.Data.ApplicationDbContext>();

        var result = await new DestinationQueryService(db).GetPageAsync(
            new DestinationQueryCriteria(Search: "patagonia"),
            CancellationToken.None);

        var destination = Assert.Single(result.Items);
        Assert.Equal("Patagonia", destination.Name);
    }
}
