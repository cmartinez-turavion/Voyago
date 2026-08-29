using Microsoft.EntityFrameworkCore;
using Voyago.Data;

using Voyago.Models.ViewModels.Catalog;
using Voyago.Models.ViewModels.Common;
namespace Voyago.Services.Queries;
public sealed class DestinationQueryService(ApplicationDbContext db) : IDestinationQueryService
{
    public async Task<PagedResult<DestinationDto>> GetPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        pageNumber = Math.Max(1, pageNumber); pageSize = Math.Clamp(pageSize, 1, 50);
        var query = db.Destinations.AsNoTracking().Where(x => x.Published);
        var total = await query.CountAsync(cancellationToken);
        var items = await query.OrderBy(x => x.Name).ThenBy(x => x.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(x => new DestinationDto(x.Id, x.Name, x.Country, x.Region, x.Category, x.Tagline, x.ImageUrl, x.Featured)).ToListAsync(cancellationToken);
        return new PagedResult<DestinationDto>(items, pageNumber, pageSize, total);
    }
}
