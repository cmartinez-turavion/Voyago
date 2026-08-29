using Microsoft.EntityFrameworkCore;
using Voyago.Data;

using Voyago.Models.ViewModels.Catalog;
using Voyago.Models.ViewModels.Common;
namespace Voyago.Services.Queries;
public sealed class HotelQueryService(ApplicationDbContext db) : IHotelQueryService
{
    public async Task<PagedResult<HotelDto>> GetPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        pageNumber = Math.Max(1, pageNumber); pageSize = Math.Clamp(pageSize, 1, 50);
        var query = db.Hotels.AsNoTracking().Where(x => x.Published);
        var total = await query.CountAsync(cancellationToken);
        var items = await query.OrderBy(x => x.Name).ThenBy(x => x.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(x => new HotelDto(x.Id, x.DestinationId, x.Name, x.Stars, x.Rating, x.PricePerNight, x.ImageUrl)).ToListAsync(cancellationToken);
        return new PagedResult<HotelDto>(items, pageNumber, pageSize, total);
    }
}
