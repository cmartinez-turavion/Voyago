using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Voyago.Models.Entities;
using Voyago.Models.ViewModels.Destinations;
using Voyago.Services.Favorites;
using Voyago.Services.Queries;

namespace Voyago.Pages.Destinations;

public sealed class IndexModel(
    IDestinationQueryService destinations,
    IFavoriteService favorites,
    UserManager<ApplicationUser> userManager) : PageModel
{
    private const int DefaultPageSize = 6;

    [BindProperty(SupportsGet = true)]
    public string? Search { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Region { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Category { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? BestSeason { get; set; }

    [BindProperty(SupportsGet = true)]
    public int PageNumber { get; set; } = 1;

    public DestinationListViewModel ViewModel { get; private set; } = null!;

    [TempData]
    public string? StatusMessage { get; set; }

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        await LoadAsync(cancellationToken);
    }

    public async Task<IActionResult> OnPostAddFavoriteAsync(
        int destinationId,
        string? returnUrl,
        CancellationToken cancellationToken)
    {
        var safeReturnUrl = GetSafeReturnUrl(returnUrl);
        var userId = userManager.GetUserId(User);

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Challenge(new AuthenticationProperties { RedirectUri = safeReturnUrl });
        }

        var result = await favorites.AddDestinationAsync(userId, destinationId, cancellationToken);
        StatusMessage = result switch
        {
            FavoriteOperationResult.Added => "Destino agregado a tus favoritos.",
            FavoriteOperationResult.AlreadyExists => "El destino ya estaba guardado en tus favoritos.",
            _ => "No fue posible guardar el destino solicitado."
        };

        return LocalRedirect(safeReturnUrl);
    }

    public async Task<IActionResult> OnPostRemoveFavoriteAsync(
        int destinationId,
        string? returnUrl,
        CancellationToken cancellationToken)
    {
        var safeReturnUrl = GetSafeReturnUrl(returnUrl);
        var userId = userManager.GetUserId(User);

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Challenge(new AuthenticationProperties { RedirectUri = safeReturnUrl });
        }

        var result = await favorites.RemoveDestinationAsync(userId, destinationId, cancellationToken);
        StatusMessage = result == FavoriteOperationResult.Removed
            ? "Destino eliminado de tus favoritos."
            : "El destino ya no se encontraba en tus favoritos.";

        return LocalRedirect(safeReturnUrl);
    }

    private async Task LoadAsync(CancellationToken cancellationToken)
    {
        var criteria = new DestinationQueryCriteria(
            Search,
            Region,
            Category,
            BestSeason,
            PageNumber,
            DefaultPageSize);

        var results = await destinations.GetPageAsync(criteria, cancellationToken);
        var options = await destinations.GetFilterOptionsAsync(cancellationToken);
        IReadOnlySet<int> favoriteIds = new HashSet<int>();
        var userId = userManager.GetUserId(User);

        if (!string.IsNullOrWhiteSpace(userId))
        {
            favoriteIds = await favorites.GetFavoriteDestinationIdsAsync(
                userId,
                results.Items.Select(x => x.Id).ToArray(),
                cancellationToken);
        }

        ViewModel = new DestinationListViewModel(criteria, options, results, favoriteIds);
    }

    private string GetSafeReturnUrl(string? returnUrl)
    {
        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return returnUrl;
        }

        return Url.Page("/Destinations/Index", new
        {
            Search,
            Region,
            Category,
            BestSeason,
            PageNumber
        }) ?? "/Destinations";
    }
}
