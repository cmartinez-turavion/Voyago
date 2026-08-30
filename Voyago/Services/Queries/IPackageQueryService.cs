using Voyago.Models.ViewModels.Catalog;
using Voyago.Models.ViewModels.Common;
using Voyago.Models.ViewModels.Home;

namespace Voyago.Services.Queries;

public interface IPackageQueryService
{
    Task<PagedResult<PackageDto>> GetPageAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<HomePackageViewModel>> GetFeaturedAsync(
        int count,
        CancellationToken cancellationToken);
}
