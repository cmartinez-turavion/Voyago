using Voyago.Models.ViewModels.Catalog;
using Voyago.Models.ViewModels.Common;
namespace Voyago.Services.Queries;
public interface IHotelQueryService { Task<PagedResult<HotelDto>> GetPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken); }
