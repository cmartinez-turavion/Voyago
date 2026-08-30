using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Voyago.Models.Entities;
using Voyago.Models.ViewModels.Destinations;
using Voyago.Services.Favorites;
using Voyago.Services.Queries;

namespace Voyago.Pages.Destinations;

public sealed class DetailsModel(
    IDestinationQueryService destinations,
    IFavoriteService favorites,
    UserManager<ApplicationUser> userManager) : PageModel
{
    public DestinationDetailViewModel Destination { get; private set; } = null!;
    public bool IsFavorite { get; private set; }

    [TempData]
    public string? StatusMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        var destination = await destinations.GetByIdAsync(id, cancellationToken);
        if (destination is null)
        {
            return NotFound();
        }

        Destination = destination;
        var userId = userManager.GetUserId(User);
        if (!string.IsNullOrWhiteSpace(userId))
        {
            IsFavorite = await favorites.IsFavoriteAsync(userId, id, cancellationToken);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAddFavoriteAsync(
        int id,
        string? returnUrl,
        CancellationToken cancellationToken)
    {
        var safeReturnUrl = GetSafeReturnUrl(id, returnUrl);
        var userId = userManager.GetUserId(User);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Challenge(new AuthenticationProperties { RedirectUri = safeReturnUrl });
        }

        var result = await favorites.AddDestinationAsync(userId, id, cancellationToken);
        StatusMessage = result switch
        {
            FavoriteOperationResult.Added => "Destino agregado a tus favoritos.",
            FavoriteOperationResult.AlreadyExists => "El destino ya estaba guardado en tus favoritos.",
            _ => "No fue posible guardar el destino solicitado."
        };
        return LocalRedirect(safeReturnUrl);
    }

    public async Task<IActionResult> OnPostRemoveFavoriteAsync(
        int id,
        string? returnUrl,
        CancellationToken cancellationToken)
    {
        var safeReturnUrl = GetSafeReturnUrl(id, returnUrl);
        var userId = userManager.GetUserId(User);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Challenge(new AuthenticationProperties { RedirectUri = safeReturnUrl });
        }

        var result = await favorites.RemoveDestinationAsync(userId, id, cancellationToken);
        StatusMessage = result == FavoriteOperationResult.Removed
            ? "Destino eliminado de tus favoritos."
            : "El destino ya no se encontraba en tus favoritos.";
        return LocalRedirect(safeReturnUrl);
    }

    private string GetSafeReturnUrl(int id, string? returnUrl)
    {
        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return returnUrl;
        }

        return Url.Page("/Destinations/Details", new { id }) ?? $"/Destinations/{id}";
    }
}
