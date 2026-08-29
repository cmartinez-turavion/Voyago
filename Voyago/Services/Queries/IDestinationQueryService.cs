using Voyago.Models.ViewModels.Catalog;
using Voyago.Models.ViewModels.Common;
namespace Voyago.Services.Queries;
public interface IDestinationQueryService { Task<PagedResult<DestinationDto>> GetPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken); }
