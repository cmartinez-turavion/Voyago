using Voyago.Models.ViewModels.Catalog;
using Voyago.Models.ViewModels.Common;
namespace Voyago.Services.Queries;
public interface IFlightQueryService { Task<PagedResult<FlightDto>> GetPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken); }
