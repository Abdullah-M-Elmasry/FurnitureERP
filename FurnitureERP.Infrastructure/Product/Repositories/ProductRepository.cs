using FurnitureERP.Application.Common.Models;
using FurnitureERP.Application.Products.Interfaces;
//using FurnitureERP.Domain.Entities.Products;
using FurnitureERP.Domain.Entities.Products;
using FurnitureERP.Infrastructure.Common;
using FurnitureERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Infrastructure.Products.Repositories;

public class ProductRepository :RepositoryBase, IProductRepository
{
    public ProductRepository(
   IDbContextFactory<AppDbContext> factory)
   : base(factory)
    {
    }

    // =====================================================
    // GET PAGED PRODUCTS
    // =====================================================

    public async Task<PagedResult<Product>> GetAll(
     string search,
     int page,
     int pageSize)
    {
        await using var db = await CreateDbContextAsync();

        var query = db.Products
            .AsNoTracking()
            .Include(x => x.Category)
            .Include(x => x.Unit)
            .Where(x => x.IsActive);

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();

            query = query.Where(x =>
                x.Name.Contains(search) ||
                x.Code.Contains(search) ||
                (x.Barcode != null &&
                 x.Barcode.Contains(search)));
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(x => x.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Product>
        {
            Items = items,
            TotalCount = totalCount
        };
    }

   
    // =====================================================
    // GET PRODUCT BY ID
    // =====================================================

    public async Task<Product?> GetById(int id)
    {
        await using var db = await CreateDbContextAsync();

        return await db.Products
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    // =====================================================
    // GET ALL ACTIVE PRODUCTS
    // =====================================================

    public async Task<List<Product>> GetLookup()
    {
        await using var db = await CreateDbContextAsync();

        return await db.Products
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    // =====================================================
    // CHECK CODE EXISTS
    // =====================================================

    public async Task<bool> CodeExists(
        string code,
        int? ignoreId = null)
    {
        await using var db = await CreateDbContextAsync();

        return await db.Products.AnyAsync(p =>
            p.Code == code &&
            (!ignoreId.HasValue ||
             p.Id != ignoreId));
    }

    // =====================================================
    // CHECK BARCODE EXISTS
    // =====================================================

    public async Task<bool> BarcodeExists(
        string barcode,
        int? ignoreId = null)
    {
        await using var db = await CreateDbContextAsync();

        return await db.Products.AnyAsync(p =>
            p.Barcode == barcode &&
            (!ignoreId.HasValue ||
             p.Id != ignoreId));
    }


    public async Task<bool> NameExists(
    string name,
    int? ignoreId = null)
    {
        await using var db = await CreateDbContextAsync();

        name = name.Trim().ToLower();

        return await db.Products.AnyAsync(x =>
            x.Name.ToLower() == name &&
            (!ignoreId.HasValue || x.Id != ignoreId));
    }


    public async Task<string> GenerateNextCode()
    {
        await using var db = await CreateDbContextAsync();

        var codes = await db.Products
            .Select(x => x.Code)
            .ToListAsync();

        var last = codes
            .Where(x => int.TryParse(x, out _))
            .Select(int.Parse)
            .DefaultIfEmpty(11110)
            .Max();

        return (last + 1).ToString("00000");
    }

    public async Task<string> GenerateNextBarcode()
    {
        await using var db = await CreateDbContextAsync();

        var last = await db.Products
            .OrderByDescending(x => x.Id)
            .Select(x => x.Barcode)
            .FirstOrDefaultAsync();

        if (string.IsNullOrWhiteSpace(last))
            return "100000000001";

        return (long.Parse(last) + 1).ToString();
    }

    // =====================================================
    // ADD PRODUCT
    // =====================================================

    public async Task Add(Product product)
    {
        await using var db = await CreateDbContextAsync();

        await db.Products.AddAsync(product);

        await db.SaveChangesAsync();
    }

    // =====================================================
    // UPDATE PRODUCT
    // =====================================================

    public async Task Update(Product product)
    {
        await using var db = await CreateDbContextAsync();

        var existing = await db.Products
            .FirstOrDefaultAsync(x => x.Id == product.Id);

        if (existing == null)
            return;

        db.Entry(existing)
           .CurrentValues
           .SetValues(product);

        await db.SaveChangesAsync();
    }

    // =====================================================
    // SOFT DELETE PRODUCT
    // =====================================================

    public async Task Delete(Product product)
    {
        await using var db = await CreateDbContextAsync();

        var existing = await db.Products
            .FirstOrDefaultAsync(x => x.Id == product.Id);

        if (existing == null)
            return;

        existing.IsActive = false;

        await db.SaveChangesAsync();
    }


    //public async Task SaveChanges()
    //{
    //    await _db.SaveChangesAsync();
    //}

  


    // =====================================================
    // GET CATEGORIES
    // =====================================================

    //public async Task<List<ProductCategory>> GetCategories()
    //{
    //    return await _db.ProductCategories
    //        .OrderBy(x => x.Name)
    //        .ToListAsync();
    //}

    //// =====================================================
    //// GET UNITS
    //// =====================================================

    //public async Task<List<Unit>> GetUnits()
    //{
    //    return await _db.Units
    //        .OrderBy(x => x.Name)
    //        .ToListAsync();
    //}

    //// =====================================================
    //// ADD CATEGORY
    //// =====================================================

    //public async Task AddCategory(ProductCategory category)
    //{
    //    await _db.ProductCategories.AddAsync(category);

    //    await _db.SaveChangesAsync();
    //}

    //// =====================================================
    //// ADD UNIT
    //// =====================================================

    //public async Task AddUnit(Unit unit)
    //{
    //    await _db.Units.AddAsync(unit);

    //    await _db.SaveChangesAsync();
    //}

    //// =====================================================
    //// CATEGORY EXISTS
    //// =====================================================

    //public async Task<bool> CategoryExists(
    //    string name,
    //    int? ignoreId = null)
    //{
    //    name = name.Trim().ToLower();

    //    return await _db.ProductCategories.AnyAsync(x =>
    //        x.Name.ToLower() == name &&
    //        (!ignoreId.HasValue ||
    //         x.Id != ignoreId));
    //}

    //// =====================================================
    //// UNIT EXISTS
    //// =====================================================

    //public async Task<bool> UnitExists(
    //    string name,
    //    int? ignoreId = null)
    //{
    //    name = name.Trim().ToLower();

    //    return await _db.Units.AnyAsync(x =>
    //        x.Name.ToLower() == name &&
    //        (!ignoreId.HasValue ||
    //         x.Id != ignoreId));
    //}

    //// =====================================================
    //// GET CATEGORY BY ID
    //// =====================================================

    //public async Task<ProductCategory?> GetCategoryById(int id)
    //{
    //    return await _db.ProductCategories
    //        .FirstOrDefaultAsync(x => x.Id == id);
    //}

    //// =====================================================
    //// UPDATE CATEGORY
    //// =====================================================

    //public async Task UpdateCategory(ProductCategory category)
    //{
    //    _db.ProductCategories.Update(category);

    //    await _db.SaveChangesAsync();
    //}

    //// =====================================================
    //// GET UNIT BY ID
    //// =====================================================

    //public async Task<Unit?> GetUnitById(int id)
    //{
    //    return await _db.Units
    //        .FirstOrDefaultAsync(x => x.Id == id);
    //}

    //// =====================================================
    //// UPDATE UNIT
    //// =====================================================

    //public async Task UpdateUnit(Unit unit)
    //{
    //    _db.Units.Update(unit);

    //    await _db.SaveChangesAsync();
    //}
}