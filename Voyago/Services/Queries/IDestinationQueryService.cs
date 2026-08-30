using Voyago.Models.ViewModels.Catalog;
using Voyago.Models.ViewModels.Common;
using Voyago.Models.ViewModels.Destinations;

namespace Voyago.Services.Queries;

public interface IDestinationQueryService
{
    Task<PagedResult<DestinationDto>> GetPageAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);

    Task<PagedResult<DestinationCardViewModel>> GetPageAsync(
        DestinationQueryCriteria criteria,
        CancellationToken cancellationToken);

    Task<DestinationDetailViewModel?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<DestinationCardViewModel>> GetFeaturedAsync(
        int count,
        CancellationToken cancellationToken);

    Task<DestinationFilterOptionsViewModel> GetFilterOptionsAsync(
        CancellationToken cancellationToken);
}
