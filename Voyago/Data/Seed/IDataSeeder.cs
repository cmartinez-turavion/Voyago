namespace Voyago.Data.Seed;
public interface IDataSeeder { Task SeedAsync(CancellationToken cancellationToken); }
