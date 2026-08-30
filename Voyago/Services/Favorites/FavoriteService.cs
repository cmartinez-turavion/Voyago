using Microsoft.EntityFrameworkCore;
using Voyago.Data;
using Voyago.Models.Entities;

namespace Voyago.Services.Favorites;

public sealed class FavoriteService(ApplicationDbContext db) : IFavoriteService
{
    public Task<bool> IsFavoriteAsync(
        string userId,
        int destinationId,
        CancellationToken cancellationToken)
    {
        ValidateUserId(userId);
        return db.Favorites.AsNoTracking().AnyAsync(
            x => x.UserId == userId && x.DestinationId == destinationId,
            cancellationToken);
    }

    public async Task<IReadOnlySet<int>> GetFavoriteDestinationIdsAsync(
        string userId,
        IReadOnlyCollection<int> destinationIds,
        CancellationToken cancellationToken)
    {
        ValidateUserId(userId);

        if (destinationIds.Count == 0)
        {
            return new HashSet<int>();
        }

        var ids = destinationIds.Distinct().ToArray();
        var favorites = await db.Favorites
            .AsNoTracking()
            .Where(x => x.UserId == userId && ids.Contains(x.DestinationId))
            .Select(x => x.DestinationId)
            .ToListAsync(cancellationToken);

        return favorites.ToHashSet();
    }

    public async Task<FavoriteOperationResult> AddDestinationAsync(
        string userId,
        int destinationId,
        CancellationToken cancellationToken)
    {
        ValidateUserId(userId);

        var destinationExists = await db.Destinations
            .AsNoTracking()
            .AnyAsync(x => x.Id == destinationId && x.Published, cancellationToken);

        if (!destinationExists)
        {
            return FavoriteOperationResult.NotFound;
        }

        var alreadyExists = await db.Favorites
            .AsNoTracking()
            .AnyAsync(
                x => x.UserId == userId && x.DestinationId == destinationId,
                cancellationToken);

        if (alreadyExists)
        {
            return FavoriteOperationResult.AlreadyExists;
        }

        db.Favorites.Add(new Favorite
        {
            UserId = userId,
            DestinationId = destinationId,
            CreatedAtUtc = DateTime.UtcNow
        });

        try
        {
            await db.SaveChangesAsync(cancellationToken);
            return FavoriteOperationResult.Added;
        }
        catch (DbUpdateException)
        {
            db.ChangeTracker.Clear();

            var wasInserted = await db.Favorites
                .AsNoTracking()
                .AnyAsync(
                    x => x.UserId == userId && x.DestinationId == destinationId,
                    cancellationToken);

            if (wasInserted)
            {
                return FavoriteOperationResult.AlreadyExists;
            }

            throw;
        }
    }

    public async Task<FavoriteOperationResult> RemoveDestinationAsync(
        string userId,
        int destinationId,
        CancellationToken cancellationToken)
    {
        ValidateUserId(userId);

        var favorite = await db.Favorites.SingleOrDefaultAsync(
            x => x.UserId == userId && x.DestinationId == destinationId,
            cancellationToken);

        if (favorite is null)
        {
            return FavoriteOperationResult.NotFound;
        }

        db.Favorites.Remove(favorite);
        await db.SaveChangesAsync(cancellationToken);
        return FavoriteOperationResult.Removed;
    }

    private static void ValidateUserId(string userId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);
    }
}
