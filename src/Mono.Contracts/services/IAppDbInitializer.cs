namespace Mono.Contracts.Services;

public interface IAppDbInitializer {
    Task EnsureDatabaseCreatedAsync();
    Task EnsureDatabaseDeletedAsync();
}