using FurnitureERP.Application.Common.Models;
using FurnitureERP.Application.Products.Interfaces;
using FurnitureERP.Domain.Entities.Products;
using FurnitureERP.Infrastructure.Common;
using FurnitureERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Infrastructure.Products.Repositories;
 public class UnitRepository
    : RepositoryBase, IUnitRepository
{
  

    public UnitRepository(
    IDbContextFactory<AppDbContext> factory)
    : base(factory)
    {
    }

    // =====================================================
    // GET PAGED
    // =====================================================

    public async Task<PagedResult<Unit>> GetAll(
        string search,
        int page,
        int pageSize)
    {
        await using var db = await CreateDbContextAsync();

        var query = db.Units
            .AsNoTracking()
            .Where(x => x.IsActive);

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();

            query = query.Where(x =>
                x.Name.Contains(search) ||
                x.Code.Contains(search));
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(x => x.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Unit>
        {
            Items = items,
            TotalCount = totalCount
        };
    }

    // =====================================================
    // GET BY ID
    // =====================================================

    public async Task<Unit?> GetById(int id)
    {
        await using var db = await CreateDbContextAsync();

        return await db.Units
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    // =====================================================
    // LOOKUP
    // =====================================================

    public async Task<List<Unit>> GetLookup()
    {
        await using var db = await CreateDbContextAsync();

        return await db.Units
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    // =====================================================
    // VALIDATION
    // =====================================================

    public async Task<bool> CodeExists(
        string code,
        int? ignoreId = null)
    {
        await using var db = await CreateDbContextAsync();

        return await db.Units.AnyAsync(x =>
            x.Code == code &&
            (!ignoreId.HasValue || x.Id != ignoreId));
    }

    public async Task<bool> NameExists(
        string name,
        int? ignoreId = null)
    {
        await using var db = await CreateDbContextAsync();

        name = name.Trim().ToLower();

        return await db.Units.AnyAsync(x =>
            x.Name.ToLower() == name &&
            (!ignoreId.HasValue || x.Id != ignoreId));
    }

    // =====================================================
    // GENERATE CODE
    // =====================================================

    public async Task<string> GenerateNextCode()
    {
        await using var db = await CreateDbContextAsync();

        var codes = await db.Units
            .Select(x => x.Code)
            .ToListAsync();

        var last = codes
            .Where(x => int.TryParse(x, out _))
            .Select(int.Parse)
            .DefaultIfEmpty(11110)
            .Max();

        return (last + 1).ToString("00000");
    }

    // =====================================================
    // CRUD
    // =====================================================

    public async Task Add(Unit unit)
    {
        await using var db = await CreateDbContextAsync();

        await db.Units.AddAsync(unit);

        await db.SaveChangesAsync();    
    }

    public async Task Update(Unit unit)
    {
        await using var db = await CreateDbContextAsync();

        var existing = await db.Units
            .FirstOrDefaultAsync(x => x.Id == unit.Id);

        if (existing == null)
            return;

        db.Entry(existing)
            .CurrentValues
            .SetValues(unit);
        await db.SaveChangesAsync();


    }

    public async Task Delete(Unit unit)
    {
        await using var db = await CreateDbContextAsync();
        var existing = await db.Units
            .FirstOrDefaultAsync(x => x.Id == unit.Id);

        if (existing == null)
            return;

        existing.IsActive = false;

        await db.SaveChangesAsync();
    }

    
}