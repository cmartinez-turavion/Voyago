using Microsoft.EntityFrameworkCore;
using Voyago.Data;
using Voyago.Models.Enums;
using Voyago.Models.ViewModels.Catalog;
using Voyago.Models.ViewModels.Common;
using Voyago.Models.ViewModels.Home;

namespace Voyago.Services.Queries;

public sealed class ReviewQueryService(ApplicationDbContext db) : IReviewQueryService
{
    public async Task<PagedResult<ReviewDto>> GetPageAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        pageNumber = Math.Max(1, pageNumber);
        pageSize = Math.Clamp(pageSize, 1, 50);
        var query = db.Reviews.AsNoTracking()
            .Where(x => x.ModerationStatus == ReviewModerationStatus.Approved);
        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(x => x.CreatedAtUtc)
            .ThenByDescending(x => x.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new ReviewDto(
                x.Id,
                x.Rating,
                x.Title,
                x.Comment,
                x.VerifiedBooking,
                x.CreatedAtUtc))
            .ToListAsync(cancellationToken);
        return new PagedResult<ReviewDto>(items, pageNumber, pageSize, total);
    }

    public async Task<IReadOnlyList<HomeReviewViewModel>> GetSelectedApprovedAsync(
        int count,
        CancellationToken cancellationToken)
    {
        count = Math.Clamp(count, 1, 12);

        return await db.Reviews
            .AsNoTracking()
            .Where(x => x.ModerationStatus == ReviewModerationStatus.Approved)
            .OrderByDescending(x => x.VerifiedBooking)
            .ThenByDescending(x => x.CreatedAtUtc)
            .ThenByDescending(x => x.Id)
            .Take(count)
            .Select(x => new HomeReviewViewModel(
                x.Id,
                x.Rating,
                x.Title,
                x.Comment,
                x.VerifiedBooking,
                x.CreatedAtUtc,
                x.Destination != null && x.Destination.Published ? x.Destination.Name : null,
                x.TourPackage != null && x.TourPackage.Published ? x.TourPackage.Title : null))
            .ToListAsync(cancellationToken);
    }
}
