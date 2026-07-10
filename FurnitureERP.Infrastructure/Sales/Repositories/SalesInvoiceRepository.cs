using FurnitureERP.Application.Common.Models;
using FurnitureERP.Application.Sales.DTOs;
using FurnitureERP.Application.Sales.Interfaces;
using FurnitureERP.Domain.Entities.Sales;
using FurnitureERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Infrastructure.Sales.Repositories;

public class SalesInvoiceRepository
    : ISalesInvoiceRepository
{
    private readonly AppDbContext _context;

    public SalesInvoiceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SalesInvoice?> GetEntityById(int id)
    {
        return await _context.SalesInvoices
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<SalesInvoiceDetailsDto?> GetById(int id)
    {
        return await _context.SalesInvoices
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new SalesInvoiceDetailsDto
            {
                Id = x.Id,

                InvoiceNumber = x.InvoiceNumber,

                Date = x.Date,

                DueDate = x.DueDate,

                CustomerId = x.CustomerId,

                CustomerName = x.Customer.Name,

                Notes = x.Notes,

                Discount = x.Discount,

                Tax = x.Tax,

                PaidAmount = x.PaidAmount,

                RemainingAmount = x.RemainingAmount,

                Status = x.Status,

                Items = x.Items
                    .Select(i => new SalesInvoiceItemDto
                    {
                        ProductId = i.ProductId,

                        ProductCode = i.Product.Code,

                        ProductName = i.Product.Name,

                        Quantity = i.Quantity,

                        SalePrice = i.SalePrice
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<PagedResult<SalesInvoiceListDto>> GetAll(
        string search,
        int page,
        int pageSize)
    {
        var query =
            _context.SalesInvoices
            .AsNoTracking()
            .Include(x => x.Customer)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();

            query = query.Where(x =>
                x.InvoiceNumber.Contains(search)
                ||
                x.Customer.Name.Contains(search));
        }

        var totalCount =
            await query.CountAsync();

        var items =
            await query
            .OrderByDescending(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new SalesInvoiceListDto
            {
                Id = x.Id,

                InvoiceNumber = x.InvoiceNumber,

                Date = x.Date,

                CustomerName = x.Customer.Name,

                Status = x.Status,

                GrandTotal = x.GrandTotal,

                PaidAmount = x.PaidAmount,

                RemainingAmount = x.RemainingAmount
            })
            .ToListAsync();

        return new PagedResult<SalesInvoiceListDto>
        {
            Items = items,
            TotalCount = totalCount
        };
    }


    public async Task Add(SalesInvoice invoice)
    {
        await _context.SalesInvoices.AddAsync(invoice);
    }

    public Task Update(SalesInvoice invoice)
    {
        _context.SalesInvoices.Update(invoice);

        return Task.CompletedTask;
    }

    public Task Delete(SalesInvoice invoice)
    {
        _context.SalesInvoices.Remove(invoice);

        return Task.CompletedTask;
    }

    public async Task<bool> InvoiceNumberExists(string invoiceNumber)
    {
        return await _context.SalesInvoices
            .AnyAsync(x => x.InvoiceNumber == invoiceNumber);
    }

    public async Task<string> GenerateNextInvoiceNumber()
    {
        var year =  DateTime.UtcNow.Year;

        var prefix = $"SI-{year}-";

        var last =
            await _context.SalesInvoices
            .Where(x => x.InvoiceNumber.StartsWith(prefix))
            .OrderByDescending(x => x.Id)
            .Select(x => x.InvoiceNumber)
            .FirstOrDefaultAsync();

        var next = 1;

        if (last != null)
        {
            var number = last.Split('-').Last();

            if (int.TryParse(number, out var seq))
                next = seq + 1;
        }

        return $"{prefix}{next:D5}";
    }

    public async Task<SalesInvoicePrintDto?> GetPrintData(int invoiceId)
    {
        return await _context.SalesInvoices
            .AsNoTracking()
            .Where(x => x.Id == invoiceId)
            .Select(x => new SalesInvoicePrintDto
            {
                Id = x.Id,

                InvoiceNumber = x.InvoiceNumber,

                Date = x.Date,

                DueDate = x.DueDate,

                CustomerName = x.Customer.Name,

                CustomerPhone = x.Customer.Phone,

                CustomerAddress = x.Customer.Address,

                Notes = x.Notes,

                Discount = x.Discount,

                Tax = x.Tax,

                SubTotal = x.SubTotal,

                GrandTotal = x.GrandTotal,

                PaidAmount = x.PaidAmount,

                RemainingAmount = x.RemainingAmount,

                Items = x.Items
                    .Select(i => new SalesInvoicePrintItemDto
                    {
                        Code = i.Product.Code,

                        Name = i.Product.Name,

                        Quantity = i.Quantity,

                        SalePrice = i.SalePrice,

                        Total = i.Quantity * i.SalePrice
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }
}