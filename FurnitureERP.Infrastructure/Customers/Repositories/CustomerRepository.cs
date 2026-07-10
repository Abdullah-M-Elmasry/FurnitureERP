using FurnitureERP.Application.Common.Models;
using FurnitureERP.Application.Customers.Interfaces;
using FurnitureERP.Domain.Entities.Customers;
using FurnitureERP.Domain.Entities.Suppliers;
using FurnitureERP.Infrastructure.Common;
using FurnitureERP.Infrastructure.Common.Extensions;
using FurnitureERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }



    public async Task<PagedResult<Customer>> GetAll(
        string search,
        int page,
        int pageSize)
    {

        var query =
            _context.Customers
            .AsNoTracking();

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

        var totalCount =
            await query.CountAsync();

        var items =
            await query
            .OrderBy(x => x.Name)
            .ApplyPaging(page, pageSize)
            .ToListAsync();

        return new PagedResult<Customer>
        {
            Items = items,
            TotalCount = totalCount
        };
    }

    public async Task<List<Customer>> GetLookup()
    {

        return await _context.Customers
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.Name)
            .ToListAsync();
    }
    public async Task<Customer?> GetById(int id)
    {

        return await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task Add(Customer customer)
    {
        _context.Customers.Add(customer);
    }

    public async Task Update(Customer customer)
    {
        _context.Customers.Update(customer);

        var existing = await _context.Customers
            .FirstOrDefaultAsync(x => x.Id == customer.Id);

        if (existing == null)
            return;

        _context.Entry(existing)
            .CurrentValues
            .SetValues(customer);
    }

    public async Task Delete(Customer customer)
    {

        var existing = await _context.Customers
            .FirstOrDefaultAsync(x => x.Id == customer.Id);

        if (existing == null)
            return;

        existing.IsActive = false;
    }

    public async Task<bool> PhoneExists(
     string phone,
     int? ignoreId = null)
    {

        phone = phone.Trim();

        return await _context.Customers.AnyAsync(x =>
            x.Phone == phone &&
            (!ignoreId.HasValue || x.Id != ignoreId));
    }

    public async Task<bool> NameExists(
    string name,
    int? ignoreId = null)
    {
        name = name.Trim().ToLower();

        return await _context.Customers.AnyAsync(x =>
            x.Name.ToLower() == name &&
            (!ignoreId.HasValue || x.Id != ignoreId));
    }

    //public async Task SaveChanges()
    //{
    //    await _db.SaveChangesAsync();
    //}
}