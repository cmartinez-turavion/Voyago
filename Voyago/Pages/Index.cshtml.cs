using Microsoft.AspNetCore.Mvc.RazorPages;
using Voyago.Models.ViewModels.Home;
using Voyago.Services.Queries;

namespace Voyago.Pages;

public sealed class IndexModel(
    IDestinationQueryService destinationQueries,
    IPackageQueryService packageQueries,
    IReviewQueryService reviewQueries) : PageModel
{
    public HomeViewModel ViewModel { get; private set; } = new([], [], []);

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        var destinationsTask = destinationQueries.GetFeaturedAsync(4, cancellationToken);
        var packagesTask = packageQueries.GetFeaturedAsync(3, cancellationToken);
        var reviewsTask = reviewQueries.GetSelectedApprovedAsync(3, cancellationToken);

        await Task.WhenAll(destinationsTask, packagesTask, reviewsTask);

        ViewModel = new HomeViewModel(
            await destinationsTask,
            await packagesTask,
            await reviewsTask);
    }
}
