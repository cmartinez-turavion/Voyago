namespace Voyago.Services.Favorites;

public interface IFavoriteService
{
    Task<bool> IsFavoriteAsync(
        string userId,
        int destinationId,
        CancellationToken cancellationToken);

    Task<IReadOnlySet<int>> GetFavoriteDestinationIdsAsync(
        string userId,
        IReadOnlyCollection<int> destinationIds,
        CancellationToken cancellationToken);

    Task<FavoriteOperationResult> AddDestinationAsync(
        string userId,
        int destinationId,
        CancellationToken cancellationToken);

    Task<FavoriteOperationResult> RemoveDestinationAsync(
        string userId,
        int destinationId,
        CancellationToken cancellationToken);
}
