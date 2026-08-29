using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Voyago.Models.Entities;

namespace Voyago.Services;

public sealed class IdentityInitializer(
    RoleManager<IdentityRole> roleManager,
    UserManager<ApplicationUser> userManager,
    IConfiguration configuration,
    ILogger<IdentityInitializer> logger) : IIdentityInitializer
{
    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        foreach (var roleName in ApplicationRoles.All)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Could not create role '{roleName}': {string.Join(", ", result.Errors.Select(error => error.Description))}");
                }
            }
        }

        var bootstrapEmail = configuration["BootstrapAdmin:Email"]?.Trim();
        if (string.IsNullOrWhiteSpace(bootstrapEmail))
        {
            return;
        }

        var normalizedEmail = userManager.NormalizeEmail(bootstrapEmail);
        var user = await userManager.Users
            .SingleOrDefaultAsync(item => item.NormalizedEmail == normalizedEmail, cancellationToken);

        if (user is null)
        {
            logger.LogWarning("Administrator bootstrap was requested, but the registered user was not found.");
            return;
        }

        if (!await userManager.IsInRoleAsync(user, ApplicationRoles.Administrator))
        {
            var result = await userManager.AddToRoleAsync(user, ApplicationRoles.Administrator);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Could not assign the Administrator role: {string.Join(", ", result.Errors.Select(error => error.Description))}");
            }

            logger.LogInformation("Administrator role assigned to the configured existing account.");
        }
    }
}
