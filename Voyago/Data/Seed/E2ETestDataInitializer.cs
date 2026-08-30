using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Voyago.Models.Entities;
using Voyago.Services;

namespace Voyago.Data.Seed;

public static class E2ETestDataInitializer
{
    public static async Task InitializeAsync(
        IServiceProvider services,
        IConfiguration configuration,
        CancellationToken cancellationToken)
    {
        await using var scope =
            services.CreateAsyncScope();

        var db = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        await db.Database.MigrateAsync(
            cancellationToken);

        var identityInitializer = scope.ServiceProvider
            .GetRequiredService<IIdentityInitializer>();

        await identityInitializer.InitializeAsync(
            cancellationToken);

        var seeder = scope.ServiceProvider
            .GetRequiredService<IDataSeeder>();

        await seeder.SeedAsync(
            cancellationToken);

        await CreateE2EUserAsync(
            scope.ServiceProvider,
            configuration);
    }

    private static async Task CreateE2EUserAsync(
        IServiceProvider services,
        IConfiguration configuration)
    {
        var email = configuration[
            "E2ETestUser:Email"]?.Trim();

        var password = configuration[
            "E2ETestUser:Password"];

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new InvalidOperationException(
                "E2ETestUser:Email is required " +
                "when the application runs in Testing.");
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException(
                "E2ETestUser:Password is required " +
                "when the application runs in Testing.");
        }

        var userManager = services
            .GetRequiredService<
                UserManager<ApplicationUser>>();

        var user = await userManager
            .FindByEmailAsync(email);

        if (user is null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = "Voyago E2E Traveler",
                EmailConfirmed = true,
                PreferredCurrency = "USD"
            };

            var createResult =
                await userManager.CreateAsync(
                    user,
                    password);

            if (!createResult.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    createResult.Errors.Select(
                        error => error.Description));

                throw new InvalidOperationException(
                    $"Could not create the E2E user: {errors}");
            }
        }

        if (!await userManager.IsInRoleAsync(
                user,
                ApplicationRoles.Traveler))
        {
            var roleResult =
                await userManager.AddToRoleAsync(
                    user,
                    ApplicationRoles.Traveler);

            if (!roleResult.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    roleResult.Errors.Select(
                        error => error.Description));

                throw new InvalidOperationException(
                    $"Could not assign Traveler: {errors}");
            }
        }

        var existingFavorites =
            await services
                .GetRequiredService<ApplicationDbContext>()
                .Favorites
                .Where(item => item.UserId == user.Id)
                .ToListAsync();

        if (existingFavorites.Count > 0)
        {
            var db = services
                .GetRequiredService<ApplicationDbContext>();

            db.Favorites.RemoveRange(
                existingFavorites);

            await db.SaveChangesAsync();
        }
    }
}