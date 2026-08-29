using Microsoft.EntityFrameworkCore;
using Voyago.Data;

using Voyago.Models.ViewModels.Catalog;
using Voyago.Models.ViewModels.Common;
namespace Voyago.Services.Queries;
public sealed class FlightQueryService(ApplicationDbContext db) : IFlightQueryService
{
    public async Task<PagedResult<FlightDto>> GetPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        pageNumber = Math.Max(1, pageNumber); pageSize = Math.Clamp(pageSize, 1, 50);
        var query = db.FlightOffers.AsNoTracking().Where(x => x.Published);
        var total = await query.CountAsync(cancellationToken);
        var items = await query.OrderBy(x => x.OriginCode).ThenBy(x => x.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(x => new FlightDto(x.Id, x.FlightType, x.Airline, x.OriginCode, x.DestinationCode, x.Duration, x.Price, x.CarbonOffsetIncluded)).ToListAsync(cancellationToken);
        return new PagedResult<FlightDto>(items, pageNumber, pageSize, total);
    }
}
