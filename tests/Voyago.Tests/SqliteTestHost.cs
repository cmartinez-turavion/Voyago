using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Voyago.Data;
using Voyago.Data.Seed;
using Voyago.Models.Entities;

namespace Voyago.Tests;

internal sealed class SqliteTestHost : IAsyncDisposable
{
    private readonly SqliteConnection _connection;

    private SqliteTestHost(SqliteConnection connection, ServiceProvider services)
    {
        _connection = connection;
        Services = services;
    }

    public ServiceProvider Services { get; }

    public static async Task<SqliteTestHost> CreateAsync()
    {
        var connection = new SqliteConnection("Data Source=:memory:;Foreign Keys=True");
        await connection.OpenAsync();

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        serviceCollection.AddSingleton(connection);
        serviceCollection.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connection));
        serviceCollection
            .AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        serviceCollection.AddScoped<IDataSeeder, DemoDataSeeder>();

        var services = serviceCollection.BuildServiceProvider();
        await using var scope = services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await db.Database.EnsureCreatedAsync();

        return new SqliteTestHost(connection, services);
    }

    public async ValueTask DisposeAsync()
    {
        await Services.DisposeAsync();
        await _connection.DisposeAsync();
    }
}
