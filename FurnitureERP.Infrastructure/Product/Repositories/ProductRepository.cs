using FurnitureERP.Application.Common.Models;
using FurnitureERP.Application.Products.Interfaces;
//using FurnitureERP.Domain.Entities.Products;
using FurnitureERP.Domain.Entities.Products;
using FurnitureERP.Infrastructure.Common;
using FurnitureERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Infrastructure.Products.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _db;

    public ProductRepository(AppDbContext db)
    {
        _db = db;
    }

    // =====================================================
    // GET PAGED PRODUCTS
    // =====================================================

    public async Task<PagedResult<Product>> GetAll(
     string search,
     int page,
     int pageSize)
    {
      

        var query = _db.Products
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
        return await _db.Products
            .AsNoTracking()
            .Include(x => x.Category)
            .Include(x => x.Unit)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    // =====================================================
    // GET ALL ACTIVE PRODUCTS
    // =====================================================

    public async Task<List<Product>> GetLookup()
    {
        return await _db.Products
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
        return await _db.Products.AnyAsync(p =>
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
        return await _db.Products.AnyAsync(p =>
            p.Barcode == barcode &&
            (!ignoreId.HasValue ||
             p.Id != ignoreId));
    }


    public async Task<bool> NameExists(
    string name,
    int? ignoreId = null)
    {
        name = name.Trim().ToLower();

        return await _db.Products.AnyAsync(x =>
            x.Name.ToLower() == name &&
            (!ignoreId.HasValue || x.Id != ignoreId));
    }


    public async Task<string> GenerateNextCode()
    {
        

        var codes = await _db.Products
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
       

        var last = await _db.Products
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
       

        await _db.Products.AddAsync(product);

    }

    // =====================================================
    // UPDATE PRODUCT
    // =====================================================

    public async Task Update(Product product)
    {

        var existing = await _db.Products
            .FirstOrDefaultAsync(x => x.Id == product.Id);

        if (existing == null)
            return;

        _db.Entry(existing)
           .CurrentValues
           .SetValues(product);

    }

    // =====================================================
    // SOFT DELETE PRODUCT
    // =====================================================

    public async Task Delete(Product product)
    {

        var existing = await _db.Products
            .FirstOrDefaultAsync(x => x.Id == product.Id);

        if (existing == null)
            return;

        existing.IsActive = false;

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