using Microsoft.EntityFrameworkCore;
using Voyago.Data;
using Voyago.Models.ViewModels.Catalog;
using Voyago.Models.ViewModels.Common;
using Voyago.Models.ViewModels.Destinations;

namespace Voyago.Services.Queries;

public sealed class DestinationQueryService(ApplicationDbContext db) : IDestinationQueryService
{
    public async Task<PagedResult<DestinationDto>> GetPageAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        pageNumber = Math.Max(1, pageNumber);
        pageSize = Math.Clamp(pageSize, 1, 50);

        var query = db.Destinations.AsNoTracking().Where(x => x.Published);
        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(x => x.Name)
            .ThenBy(x => x.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new DestinationDto(
                x.Id,
                x.Name,
                x.Country,
                x.Region,
                x.Category,
                x.Tagline,
                x.ImageUrl,
                x.Featured))
            .ToListAsync(cancellationToken);

        return new PagedResult<DestinationDto>(items, pageNumber, pageSize, total);
    }

    public async Task<PagedResult<DestinationCardViewModel>> GetPageAsync(
        DestinationQueryCriteria criteria,
        CancellationToken cancellationToken)
    {
        var pageNumber = Math.Max(1, criteria.PageNumber);
        var pageSize = Math.Clamp(criteria.PageSize, 1, 50);
        var search = Normalize(criteria.Search);
        var region = Normalize(criteria.Region);
        var category = Normalize(criteria.Category);
        var bestSeason = Normalize(criteria.BestSeason);

        var query = db.Destinations.AsNoTracking().Where(x => x.Published);

        if (search is not null)
        {
            var pattern = $"%{EscapeLikePattern(search)}%";

            query = query.Where(x =>
                EF.Functions.Like(x.Name, pattern, "\\") ||
                EF.Functions.Like(x.Country, pattern, "\\") ||
                EF.Functions.Like(x.Region, pattern, "\\"));
        }

        if (region is not null)
        {
            query = query.Where(x => x.Region == region);
        }

        if (category is not null)
        {
            query = query.Where(x => x.Category == category);
        }

        if (bestSeason is not null)
        {
            query = query.Where(x => x.BestSeason == bestSeason);
        }

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(x => x.Featured)
            .ThenBy(x => x.Name)
            .ThenBy(x => x.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new DestinationCardViewModel(
                x.Id,
                x.Name,
                x.Country,
                x.Region,
                x.Category,
                x.Tagline,
                x.ImageUrl,
                x.BestSeason,
                x.RecommendedDuration,
                x.Featured))
            .ToListAsync(cancellationToken);

        return new PagedResult<DestinationCardViewModel>(items, pageNumber, pageSize, total);
    }

    public async Task<DestinationDetailViewModel?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            return null;
        }

        var destination = await db.Destinations
            .AsNoTracking()
            .Where(x => x.Id == id && x.Published)
            .Select(x => new
            {
                x.Id,
                x.Name,
                x.Country,
                x.Region,
                x.Category,
                x.Tagline,
                x.Description,
                x.ImageUrl,
                x.Latitude,
                x.Longitude,
                x.BestSeason,
                x.RecommendedDuration,
                x.Featured
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (destination is null)
        {
            return null;
        }

        var packages = await db.TourPackages
            .AsNoTracking()
            .Where(x => x.DestinationId == id && x.Published)
            .OrderByDescending(x => x.Featured)
            .ThenBy(x => x.Title)
            .ThenBy(x => x.Id)
            .Take(4)
            .Select(x => new DestinationRelatedPackageViewModel(
                x.Id,
                x.Title,
                x.DurationDays,
                x.TravelStyle,
                x.PricePerPerson,
                x.OriginalPrice,
                x.ImageUrl,
                x.AvailableSlots,
                x.Featured))
            .ToListAsync(cancellationToken);

        var hotels = await db.Hotels
            .AsNoTracking()
            .Where(x => x.DestinationId == id && x.Published)
            .OrderByDescending(x => x.Stars)
            .ThenBy(x => x.Name)
            .ThenBy(x => x.Id)
            .Take(4)
            .Select(x => new DestinationRelatedHotelViewModel(
                x.Id,
                x.Name,
                x.Stars,
                x.Rating,
                x.PricePerNight,
                x.ImageUrl))
            .ToListAsync(cancellationToken);

        return new DestinationDetailViewModel(
            destination.Id,
            destination.Name,
            destination.Country,
            destination.Region,
            destination.Category,
            destination.Tagline,
            destination.Description,
            destination.ImageUrl,
            destination.Latitude,
            destination.Longitude,
            destination.BestSeason,
            destination.RecommendedDuration,
            destination.Featured,
            packages,
            hotels);
    }

    public async Task<IReadOnlyList<DestinationCardViewModel>> GetFeaturedAsync(
        int count,
        CancellationToken cancellationToken)
    {
        count = Math.Clamp(count, 1, 12);

        return await db.Destinations
            .AsNoTracking()
            .Where(x => x.Published && x.Featured)
            .OrderBy(x => x.Name)
            .ThenBy(x => x.Id)
            .Take(count)
            .Select(x => new DestinationCardViewModel(
                x.Id,
                x.Name,
                x.Country,
                x.Region,
                x.Category,
                x.Tagline,
                x.ImageUrl,
                x.BestSeason,
                x.RecommendedDuration,
                x.Featured))
            .ToListAsync(cancellationToken);
    }

    public async Task<DestinationFilterOptionsViewModel> GetFilterOptionsAsync(
        CancellationToken cancellationToken)
    {
        var published = db.Destinations.AsNoTracking().Where(x => x.Published);

        var regions = await published
            .Where(x => x.Region != string.Empty)
            .Select(x => x.Region)
            .Distinct()
            .OrderBy(x => x)
            .ToListAsync(cancellationToken);

        var categories = await published
            .Where(x => x.Category != string.Empty)
            .Select(x => x.Category)
            .Distinct()
            .OrderBy(x => x)
            .ToListAsync(cancellationToken);

        var bestSeasons = await published
            .Where(x => x.BestSeason != string.Empty)
            .Select(x => x.BestSeason)
            .Distinct()
            .OrderBy(x => x)
            .ToListAsync(cancellationToken);

        return new DestinationFilterOptionsViewModel(regions, categories, bestSeasons);
    }

    private static string? Normalize(string? value)
    {
        var normalized = value?.Trim();
        return string.IsNullOrWhiteSpace(normalized) ? null : normalized;
    }

    private static string EscapeLikePattern(string value) => value
        .Replace("\\", "\\\\", StringComparison.Ordinal)
        .Replace("%", "\\%", StringComparison.Ordinal)
        .Replace("_", "\\_", StringComparison.Ordinal);
}
