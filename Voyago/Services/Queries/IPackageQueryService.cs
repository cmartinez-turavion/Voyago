using Voyago.Models.ViewModels.Catalog;
using Voyago.Models.ViewModels.Common;
namespace Voyago.Services.Queries;
public interface IPackageQueryService { Task<PagedResult<PackageDto>> GetPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken); }
