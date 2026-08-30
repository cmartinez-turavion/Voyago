using Voyago.Models.ViewModels.Common;

namespace Voyago.Models.ViewModels.Destinations;

public sealed record DestinationListViewModel(
    DestinationQueryCriteria Criteria,
    DestinationFilterOptionsViewModel FilterOptions,
    PagedResult<DestinationCardViewModel> Results,
    IReadOnlySet<int> FavoriteDestinationIds);
