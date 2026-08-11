using FurnitureERP.Application.Common.Models;
using FurnitureERP.Application.Products.Interfaces;
using FurnitureERP.Domain.Entities.Products;
using FurnitureERP.Infrastructure.Common;
using FurnitureERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Infrastructure.Products.Repositories;

public class CategoryRepository : ICategoryRepository
{
    
    private readonly AppDbContext _db;

    public CategoryRepository(AppDbContext db)
    {
        _db = db;
    }

    // =====================================================
    // GET PAGED CATEGORIES
    // =====================================================

    public async Task<PagedResult<ProductCategory>> GetAll(
        string search,
        int page,
        int pageSize)
    {

        var query = _db.ProductCategories
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

        return new PagedResult<ProductCategory>
        {
            Items = items,
            TotalCount = totalCount
        };
    }

    // =====================================================
    // LOOKUP
    // =====================================================

    public async Task<List<ProductCategory>> GetLookup()
    {

        return await _db.ProductCategories
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    // =====================================================
    // GET BY ID
    // =====================================================

    public async Task<ProductCategory?> GetById(int id)
    {
        return await _db.ProductCategories
            .FirstOrDefaultAsync(x => x.Id == id);
    }

   

    // =====================================================
    // VALIDATION
    // =====================================================

    public async Task<bool> CodeExists(
        string code,
        int? ignoreId = null)
    {
        return await _db.ProductCategories.AnyAsync(x =>
            x.Code == code &&
            (!ignoreId.HasValue || x.Id != ignoreId));
    }

    public async Task<bool> NameExists(
        string name,
        int? ignoreId = null)
    {

        name = name.Trim().ToLower();

        return await _db.ProductCategories.AnyAsync(x =>
            x.Name.ToLower() == name  &&
            (!ignoreId.HasValue || x.Id != ignoreId));
    }

    // =====================================================
    // GENERATE CODE
    // =====================================================

    public async Task<string> GenerateNextCode()
    {

        var codes = await _db.ProductCategories
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

    public async Task Add(ProductCategory category)
    {

        await _db.ProductCategories.AddAsync(category);

    }

    public async Task Update(ProductCategory category)
    {
        var existing = await _db.ProductCategories
            .FirstOrDefaultAsync(x => x.Id == category.Id);

        if (existing == null)
            return;

        _db.Entry(existing)
            .CurrentValues
            .SetValues(category);

    }

    public async Task Delete(ProductCategory category)
    {

        var existing = await _db.ProductCategories
            .FirstOrDefaultAsync(x => x.Id == category.Id);

        if (existing == null)
            return;

        existing.IsActive = false;
    }

    //public async Task SaveChanges()
    //{
    //    await _db.SaveChangesAsync();
    //}
}