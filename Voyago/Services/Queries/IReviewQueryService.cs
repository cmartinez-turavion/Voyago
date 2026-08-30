using Voyago.Models.ViewModels.Catalog;
using Voyago.Models.ViewModels.Common;
using Voyago.Models.ViewModels.Home;

namespace Voyago.Services.Queries;

public interface IReviewQueryService
{
    Task<PagedResult<ReviewDto>> GetPageAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<HomeReviewViewModel>> GetSelectedApprovedAsync(
        int count,
        CancellationToken cancellationToken);
}
