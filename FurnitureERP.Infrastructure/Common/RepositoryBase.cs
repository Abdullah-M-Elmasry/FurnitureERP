using FurnitureERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Infrastructure.Common;

public abstract class RepositoryBase
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    protected RepositoryBase(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    protected Task<AppDbContext> CreateDbContextAsync()
    {
        return _factory.CreateDbContextAsync();
    }
}