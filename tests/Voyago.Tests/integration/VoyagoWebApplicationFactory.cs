using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Voyago.Data;
using Voyago.Data.Seed;
using Voyago.Services;

namespace Voyago.Tests.Integration;

public sealed class VoyagoWebApplicationFactory
    : WebApplicationFactory<Program>
{
    private readonly string _databasePath;

    public VoyagoWebApplicationFactory()
    {
        var databaseName =
            $"voyago-tests-{Guid.NewGuid():N}.db";

        _databasePath = Path.Combine(
            Path.GetTempPath(),
            databaseName);
    }

    public string DatabasePath => _databasePath;

    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        builder.UseEnvironment(
            "IntegrationTesting");

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<
                DbContextOptions<ApplicationDbContext>>();

            services.RemoveAll<
                ApplicationDbContext>();

            var connectionString =
                $"Data Source={_databasePath};Foreign Keys=True";

            services.AddDbContext<ApplicationDbContext>(
                options =>
                    options.UseSqlite(connectionString));
        });
    }

    protected override IHost CreateHost(
        IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        InitializeDatabaseAsync(host.Services)
            .GetAwaiter()
            .GetResult();

        return host;
    }

    private static async Task InitializeDatabaseAsync(
        IServiceProvider services)
    {
        await using var scope =
            services.CreateAsyncScope();

        var db = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        await db.Database.MigrateAsync();

        var identityInitializer =
            scope.ServiceProvider
                .GetRequiredService<IIdentityInitializer>();

        await identityInitializer.InitializeAsync(
            CancellationToken.None);

        var seeder =
            scope.ServiceProvider
                .GetRequiredService<IDataSeeder>();

        await seeder.SeedAsync(
            CancellationToken.None);
    }

    protected override void Dispose(
        bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            DeleteDatabaseFiles();
        }
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();

        DeleteDatabaseFiles();
    }

    private void DeleteDatabaseFiles()
    {
        DeleteIfExists(_databasePath);
        DeleteIfExists($"{_databasePath}-shm");
        DeleteIfExists($"{_databasePath}-wal");
    }

    private static void DeleteIfExists(
        string path)
    {
        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        catch (IOException)
        {
            // La limpieza temporal no debe
            // ocultar el resultado de la prueba.
        }
        catch (UnauthorizedAccessException)
        {
            // SQLite puede liberar el archivo
            // algunos instantes después.
        }
    }
}