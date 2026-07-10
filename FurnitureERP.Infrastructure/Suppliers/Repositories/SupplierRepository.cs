using FurnitureERP.Application.Common.Models;
using FurnitureERP.Application.Suppliers.Interfaces;
using FurnitureERP.Domain.Entities.Suppliers;
using FurnitureERP.Infrastructure.Common;
using FurnitureERP.Infrastructure.Common.Extensions;
using FurnitureERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;


namespace FurnitureERP.Infrastructure.Suppliers.Repositories;

public class SupplierRepository :RepositoryBase ,ISupplierRepository
{
    public SupplierRepository(
   IDbContextFactory<AppDbContext> factory)
   : base(factory)
    {
    }

    public async Task<PagedResult<Supplier>> GetAll(
    string search,
    int page,
    int pageSize)
    {
        await using var db = await CreateDbContextAsync();
        var query = db.Suppliers
    .AsNoTracking()
    .Where(x => x.IsActive);

        if (!string.IsNullOrWhiteSpace(search))
    {
        search = search.Trim();

        query = query.Where(x =>

            x.Name.Contains(search)

            ||

            (x.Phone != null &&
             x.Phone.Contains(search))

            ||

            (x.Email != null &&
             x.Email.Contains(search))
        );
    }

    // مهم: نحسب العدد قبل الـ paging
    var totalCount =
        await query.CountAsync();

    // بعدها فقط نطبق Paging
    var items =
        await query
        .OrderBy(x => x.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        //.OrderBy(x => x.Name)
        //.ApplyPaging(page, pageSize)
        //.ToListAsync();

        return new PagedResult<Supplier>
    {
        Items = items,
        TotalCount = totalCount
    };
}

    public async Task<List<Supplier>> GetLookup()
    {
        await using var db = await CreateDbContextAsync();

        return await db.Suppliers
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<Supplier?> GetById(
        int id)
    {
        await using var db = await CreateDbContextAsync();
        return await db.Suppliers
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == id);
    }

    public async Task Add(
        Supplier supplier)
    {
        await using var db = await CreateDbContextAsync();
        await db.Suppliers
            .AddAsync(supplier);

        await  db.SaveChangesAsync();
    }

    public async Task Update(
        Supplier supplier)
    {
        await using var db = await CreateDbContextAsync();

        var existing = await db.Suppliers
            .FirstOrDefaultAsync(
                x => x.Id == supplier.Id);

        if (existing == null)
            return;

        db.Entry(existing)
            .CurrentValues
            .SetValues(supplier);

        await db.SaveChangesAsync();
    }

    public async Task Delete(
        Supplier supplier)
    {
        await using var db = await CreateDbContextAsync();
        var existing = await db.Suppliers
            .FirstOrDefaultAsync(
                x => x.Id == supplier.Id);

        if (existing == null)
            return;

        existing.IsActive = false;

        await db.SaveChangesAsync();
    }

    public async Task<bool> NameExists(
     string name,
     int? ignoreId = null)
    {
        await using var db = await CreateDbContextAsync();

        name = name.Trim().ToLower();

        return await db.Suppliers.AnyAsync(x =>
            x.Name.ToLower() == name &&
            (!ignoreId.HasValue || x.Id != ignoreId));
    }

    public async Task<bool> PhoneExists(
     string phone,
     int? ignoreId = null)
    {
        await using var db = await CreateDbContextAsync();

        phone = phone.Trim();

        return await db.Suppliers.AnyAsync(x =>
            x.Phone == phone &&
            (!ignoreId.HasValue || x.Id != ignoreId));
    }
}