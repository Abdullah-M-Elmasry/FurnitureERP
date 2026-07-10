using FurnitureERP.Application.Common.Models;
using FurnitureERP.Application.Purchases.DTOs;
using FurnitureERP.Application.Purchases.Interfaces;
using FurnitureERP.Infrastructure.Persistence;

//using FurnitureERP.Domain.Entities.Purchases;
//using FurnitureERP.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Infrastructure.Purchases.Repositories;

public class PurchaseInvoiceRepository
    : IPurchaseInvoiceRepository
{
    private readonly AppDbContext _context;

    public PurchaseInvoiceRepository(
        AppDbContext context)
    {
        _context = context;
    }

    public async Task<PurchaseInvoice?> GetEntityById(
    int id)
    {
        return await _context.PurchaseInvoices
    .Include(x => x.Items)
    .ThenInclude(x => x.Product)
    .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<PurchaseInvoiceDetailsDto?> GetById(
    int id)
    {
        return await _context.PurchaseInvoices
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new PurchaseInvoiceDetailsDto
            {
                Id = x.Id,
                InvoiceNumber = x.InvoiceNumber,
                Date = x.Date,
                DueDate = x.DueDate,
                SupplierId = x.SupplierId,
                SupplierName = x.Supplier.Name,
                Notes = x.Notes,
                Discount = x.Discount,
                Tax = x.Tax,
                Status = x.Status,

                Items = x.Items
                    .Select(i =>
                        new PurchaseInvoiceItemDto
                        {
                            ProductId = i.ProductId,
                            ProductCode = i.Product.Code,
                            ProductName = i.Product.Name,
                            Quantity = i.Quantity,
                            CostPrice = i.CostPrice
                        })
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<PagedResult<PurchaseInvoiceListDto>> GetAll(
    string search,
    int page,
    int pageSize)
    {
        var query =
            _context.PurchaseInvoices
                .AsNoTracking()
                .Include(x => x.Supplier)
                .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();

            query = query.Where(x =>
                x.InvoiceNumber.Contains(search)
                ||
                x.Supplier.Name.Contains(search));
        }

        var totalCount =
            await query.CountAsync();

        var items =
            await query
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x =>
                    new PurchaseInvoiceListDto
                    {
                        Id = x.Id,
                        InvoiceNumber = x.InvoiceNumber,
                        Date = x.Date,
                        SupplierName = x.Supplier.Name,
                        Status = x.Status,
                        GrandTotal = x.GrandTotal
                    })
                .ToListAsync();

        return new PagedResult<PurchaseInvoiceListDto>
        {
            Items = items,
            TotalCount = totalCount
        };
    }

    public async Task Add(
    PurchaseInvoice invoice)
    {
        await _context.PurchaseInvoices
            .AddAsync(invoice);
    }

    public Task Update(
        PurchaseInvoice invoice)
    {
        _context.PurchaseInvoices.Update(invoice);

        return Task.CompletedTask;
    }

    public Task Delete(
        PurchaseInvoice invoice)
    {
        _context.PurchaseInvoices.Remove(invoice);

        return Task.CompletedTask;
    }

    public async Task<string> GenerateNextInvoiceNumber()
    {
        var year =  DateTime.UtcNow.Year;

        var prefix = $"PI-{year}-";

        var last =
            await _context.PurchaseInvoices
                .Where(x =>
                    x.InvoiceNumber.StartsWith(prefix))
                .OrderByDescending(x => x.Id)
                .Select(x => x.InvoiceNumber)
                .FirstOrDefaultAsync();

        var next = 1;

        if (last != null)
        {
            var number =
                last.Split('-').Last();

            if (int.TryParse(number, out var seq))
                next = seq + 1;
        }

        return $"{prefix}{next:D5}";
    }


   

    public async Task<bool> InvoiceNumberExists(
    string invoiceNumber)
    {
        return await _context.PurchaseInvoices
            .AnyAsync(x =>
                x.InvoiceNumber == invoiceNumber);
    }


    public async Task<PurchaseInvoicePrintDto?> GetPrintData(int invoiceId)
    {
        return await _context.PurchaseInvoices
            .AsNoTracking()
            .Where(x => x.Id == invoiceId)
            .Select(x => new PurchaseInvoicePrintDto
            {
                Id = x.Id,

                InvoiceNumber = x.InvoiceNumber,

                Date = x.Date,

                DueDate = x.DueDate,

                SupplierName = x.Supplier.Name,

                SupplierPhone = x.Supplier.Phone,

                SupplierAddress = x.Supplier.Address,

                Notes = x.Notes,

                Discount = x.Discount,

                Tax = x.Tax,

                SubTotal = x.SubTotal,

                GrandTotal = x.GrandTotal,

                Items = x.Items.Select(i =>
                    new PurchaseInvoicePrintItemDto
                    {
                        Code = i.Product.Code,

                        Name = i.Product.Name,

                        Quantity = i.Quantity,

                        CostPrice = i.CostPrice,

                        Total = i.Quantity * i.CostPrice
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }
}