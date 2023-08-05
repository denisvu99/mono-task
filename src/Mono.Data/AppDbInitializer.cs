using Mono.Data;
using Mono.Contracts.Services;

public class AppDbInitializer : IAppDbInitializer
{
    private readonly AppDbContext _db;

    public AppDbInitializer(AppDbContext db)
    {
        _db = db;
    }

    public async Task EnsureDatabaseCreatedAsync()
    {
        await _db.Database.EnsureCreatedAsync();
    }

    public async Task EnsureDatabaseDeletedAsync()
    {
        await _db.Database.EnsureDeletedAsync();
    }
}
