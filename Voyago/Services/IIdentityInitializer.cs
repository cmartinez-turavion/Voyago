namespace Voyago.Services;

public interface IIdentityInitializer
{
    Task InitializeAsync(CancellationToken cancellationToken);
}
