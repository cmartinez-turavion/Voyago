using Microsoft.EntityFrameworkCore;
using Voyago.Data;
using Voyago.Models.Enums;
using Voyago.Models.ViewModels.Catalog;
using Voyago.Models.ViewModels.Common;
namespace Voyago.Services.Queries;
public sealed class ReviewQueryService(ApplicationDbContext db) : IReviewQueryService
{
    public async Task<PagedResult<ReviewDto>> GetPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        pageNumber = Math.Max(1, pageNumber); pageSize = Math.Clamp(pageSize, 1, 50);
        var query = db.Reviews.AsNoTracking().Where(x => x.ModerationStatus == ReviewModerationStatus.Approved);
        var total = await query.CountAsync(cancellationToken);
        var items = await query.OrderBy(x => x.CreatedAtUtc).ThenBy(x => x.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(x => new ReviewDto(x.Id, x.Rating, x.Title, x.Comment, x.VerifiedBooking, x.CreatedAtUtc)).ToListAsync(cancellationToken);
        return new PagedResult<ReviewDto>(items, pageNumber, pageSize, total);
    }
}
