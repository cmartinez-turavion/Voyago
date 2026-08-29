using Voyago.Models.ViewModels.Catalog;
using Voyago.Models.ViewModels.Common;
namespace Voyago.Services.Queries;
public interface IReviewQueryService { Task<PagedResult<ReviewDto>> GetPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken); }
