using FurnitureERP.Application.Common.Models;
using FurnitureERP.Application.Products.Interfaces;
using FurnitureERP.Domain.Entities.Products;
using FurnitureERP.Infrastructure.Common;
using FurnitureERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Infrastructure.Products.Repositories;

public class CategoryRepository :RepositoryBase ,ICategoryRepository
{
    public CategoryRepository(
   IDbContextFactory<AppDbContext> factory)
   : base(factory)
    {
    }

    // =====================================================
    // GET PAGED CATEGORIES
    // =====================================================

    public async Task<PagedResult<ProductCategory>> GetAll(
        string search,
        int page,
        int pageSize)
    {
        await using var db = await CreateDbContextAsync();

        var query = db.ProductCategories
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
        await using var db = await CreateDbContextAsync();

        return await db.ProductCategories
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
        await using var db = await CreateDbContextAsync();

        return await db.ProductCategories
            .FirstOrDefaultAsync(x => x.Id == id);
    }

   

    // =====================================================
    // VALIDATION
    // =====================================================

    public async Task<bool> CodeExists(
        string code,
        int? ignoreId = null)
    {
        await using var db = await CreateDbContextAsync();

        return await db.ProductCategories.AnyAsync(x =>
            x.Code == code &&
            (!ignoreId.HasValue || x.Id != ignoreId));
    }

    public async Task<bool> NameExists(
        string name,
        int? ignoreId = null)
    {
        await using var db = await CreateDbContextAsync();

        name = name.Trim().ToLower();

        return await db.ProductCategories.AnyAsync(x =>
            x.Name.ToLower() == name  &&
            (!ignoreId.HasValue || x.Id != ignoreId));
    }

    // =====================================================
    // GENERATE CODE
    // =====================================================

    public async Task<string> GenerateNextCode()
    {
        await using var db = await CreateDbContextAsync();

        var codes = await db.ProductCategories
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
        await using var db = await CreateDbContextAsync();

        await db.ProductCategories.AddAsync(category);

        await db.SaveChangesAsync();

    }

    public async Task Update(ProductCategory category)
    {
        await using var db = await CreateDbContextAsync();
        var existing = await db.ProductCategories
            .FirstOrDefaultAsync(x => x.Id == category.Id);

        if (existing == null)
            return;

        db.Entry(existing)
            .CurrentValues
            .SetValues(category);

        await db.SaveChangesAsync();
    }

    public async Task Delete(ProductCategory category)
    {
        await using var db = await CreateDbContextAsync();

        var existing = await db.ProductCategories
            .FirstOrDefaultAsync(x => x.Id == category.Id);

        if (existing == null)
            return;

        existing.IsActive = false;

        await db.SaveChangesAsync();
    }

    //public async Task SaveChanges()
    //{
    //    await _db.SaveChangesAsync();
    //}
}