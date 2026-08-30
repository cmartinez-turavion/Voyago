using Microsoft.EntityFrameworkCore;
using Voyago.Data;
using Voyago.Models.ViewModels.Catalog;
using Voyago.Models.ViewModels.Common;
using Voyago.Models.ViewModels.Home;

namespace Voyago.Services.Queries;

public sealed class PackageQueryService(ApplicationDbContext db) : IPackageQueryService
{
    public async Task<PagedResult<PackageDto>> GetPageAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        pageNumber = Math.Max(1, pageNumber);
        pageSize = Math.Clamp(pageSize, 1, 50);
        var query = db.TourPackages.AsNoTracking().Where(x => x.Published);
        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(x => x.Title)
            .ThenBy(x => x.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new PackageDto(
                x.Id,
                x.DestinationId,
                x.Title,
                x.DurationDays,
                x.TravelStyle,
                x.PricePerPerson,
                x.ImageUrl,
                x.AvailableSlots,
                x.Featured))
            .ToListAsync(cancellationToken);
        return new PagedResult<PackageDto>(items, pageNumber, pageSize, total);
    }

    public async Task<IReadOnlyList<HomePackageViewModel>> GetFeaturedAsync(
        int count,
        CancellationToken cancellationToken)
    {
        count = Math.Clamp(count, 1, 12);

        return await db.TourPackages
            .AsNoTracking()
            .Where(x => x.Published && x.Featured && x.Destination.Published)
            .OrderBy(x => x.Title)
            .ThenBy(x => x.Id)
            .Take(count)
            .Select(x => new HomePackageViewModel(
                x.Id,
                x.DestinationId,
                x.Destination.Name,
                x.Title,
                x.DurationDays,
                x.TravelStyle,
                x.PricePerPerson,
                x.ImageUrl,
                x.AvailableSlots))
            .ToListAsync(cancellationToken);
    }
}
