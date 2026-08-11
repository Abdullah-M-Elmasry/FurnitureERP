using FurnitureERP.Application.Common.Models;
using FurnitureERP.Application.Products.Interfaces;
using FurnitureERP.Domain.Entities.Products;
using FurnitureERP.Infrastructure.Common;
using FurnitureERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Infrastructure.Products.Repositories;
 public class UnitRepository: IUnitRepository
{
  
    private readonly AppDbContext _db;

    public UnitRepository(AppDbContext db)
    {
        _db = db;
    }
    // =====================================================
    // GET PAGED
    // =====================================================

    public async Task<PagedResult<Unit>> GetAll(
        string search,
        int page,
        int pageSize)
    {

        var query = _db.Units
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
        return await _db.Units
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    // =====================================================
    // LOOKUP
    // =====================================================

    public async Task<List<Unit>> GetLookup()
    {
        return await _db.Units
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
        return await _db.Units.AnyAsync(x =>
            x.Code == code &&
            (!ignoreId.HasValue || x.Id != ignoreId));
    }

    public async Task<bool> NameExists(
        string name,
        int? ignoreId = null)
    {

        name = name.Trim().ToLower();

        return await _db.Units.AnyAsync(x =>
            x.Name.ToLower() == name &&
            (!ignoreId.HasValue || x.Id != ignoreId));
    }

    // =====================================================
    // GENERATE CODE
    // =====================================================

    public async Task<string> GenerateNextCode()
    {
        var codes = await _db.Units
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

        await _db.Units.AddAsync(unit);
 
    }

    public async Task Update(Unit unit)
    {

        var existing = await _db.Units
            .FirstOrDefaultAsync(x => x.Id == unit.Id);

        if (existing == null)
            return;

        _db.Entry(existing)
            .CurrentValues
            .SetValues(unit);


    }

    public async Task Delete(Unit unit)
    {
    
        var existing = await _db.Units
            .FirstOrDefaultAsync(x => x.Id == unit.Id);

        if (existing == null)
            return;

        existing.IsActive = false;

    }

    
}