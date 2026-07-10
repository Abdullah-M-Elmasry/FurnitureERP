using FurnitureERP.Application.Common.Models;
using FurnitureERP.Application.Inventory.DTOs;
using FurnitureERP.Application.Inventory.Interfaces;
using FurnitureERP.Application.Products.DTOs;
using FurnitureERP.Domain.Entities.Inventories;
using FurnitureERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Infrastructure.Inventory.Repositories;

public class InventoryRepository : IInventoryRepository
{
    private readonly AppDbContext _context;

    public InventoryRepository(AppDbContext context)
    {
        _context = context;
    }

    // =========================================================
    // Query Builder
    // =========================================================

    private IQueryable<ProductInventory> BuildInventoryQuery()
    {
        return _context.Inventories
            .Include(x => x.Product)
                .ThenInclude(x => x.Category)
            .Include(x => x.Product)
                .ThenInclude(x => x.Unit)
            .AsNoTracking();
    }
    private IQueryable<InventoryTransaction> BuildTransactionQuery()
    {
        return _context.InventoryTransactions
            .Include(x => x.Product)
            .AsNoTracking();
    }

    // =========================================================
    // Transactions
    // =========================================================

    public async Task<PagedResult<InventoryItemDto>> GetAll(
     string search,
     int page,
     int pageSize)
    {
        var query = BuildInventoryQuery();

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();

            query = query.Where(x =>
                x.Product.Name.Contains(search) ||
                x.Product.Code.Contains(search) ||
                (x.Product.Barcode != null && x.Product.Barcode.Contains(search)));
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(x => x.Product.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new InventoryItemDto
            {
                ProductId = x.ProductId,

                Code = x.Product.Code,

                Barcode = x.Product.Barcode,

                Name = x.Product.Name,

                Category = x.Product.Category.Name,

                Unit = x.Product.Unit.Name,

                CurrentStock = x.CurrentQuantity,

                UpdatedAt = x.UpdatedAt,

                //Status =
                //    x.CurrentQuantity == 0
                //        ? "Out Of Stock"
                //        : x.CurrentQuantity <= 5
                //            ? "Low Stock"
                //            : "Available"
            })
            .ToListAsync();

        return new PagedResult<InventoryItemDto>
        {
            Items = items,
            TotalCount = totalCount
        };
    }

    public async Task<PagedResult<InventoryTransactionDto>> GetProductTransactions(
     int productId,
     string search,
     int page,
     int pageSize)
    {
        var query = BuildTransactionQuery()
            .Where(x => x.ProductId == productId);

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();

            query = query.Where(x =>
                (x.Notes != null && x.Notes.Contains(search)) ||
                x.Type.ToString().Contains(search) ||
                (x.ReferenceType != null &&
                 x.ReferenceType.ToString()!.Contains(search)) ||
                (x.ReferenceId != null &&
                 x.ReferenceId.ToString()!.Contains(search)));
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(x => x.TransactionDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new InventoryTransactionDto
            {
                Id = x.Id,

                TransactionDate = x.TransactionDate,

                Type = x.Type,

                Quantity = x.Quantity,

                BalanceAfter = x.BalanceAfter,

                ReferenceId = x.ReferenceId,

                ReferenceType = x.ReferenceType ,

                AdjustmentReason = x.AdjustmentReason,

                Notes = x.Notes,

                CreatedBy = x.CreatedBy
            })
            .ToListAsync();

        return new PagedResult<InventoryTransactionDto>
        {
            Items = items,
            TotalCount = totalCount
        };
    }

    // =========================================================
    // Inventory
    // =========================================================

    public async Task<ProductInventory?> GetByProductId(
        int productId)
    {
        return await _context.Inventories
    .Include(x => x.Product)
    .FirstOrDefaultAsync(x => x.ProductId == productId);
    }

    public async Task<decimal> GetCurrentStock(
        int productId)
    {
        return await _context.Inventories
            .Where(x => x.ProductId == productId)
            .Select(x => x.CurrentQuantity)
            .FirstOrDefaultAsync();
    }

    public async Task<ProductInventory> GetOrCreateInventory(
        int productId)
    {
        var inventory = await GetByProductId(productId);

        if (inventory != null)
            return inventory;

        inventory = new ProductInventory
        {
            ProductId = productId,
            CurrentQuantity = 0
        };

        await _context.Inventories.AddAsync(inventory);

        return inventory;
    }

    public async Task AddInventory(
        ProductInventory inventory)
    {
        await _context.Inventories.AddAsync(inventory);
    }

    public Task UpdateInventory(
        ProductInventory inventory)
    {
        _context.Inventories.Update(inventory);

        return Task.CompletedTask;
    }

    public async Task AddTransaction(
        InventoryTransaction transaction)
    {
        await _context.InventoryTransactions.AddAsync(transaction);
    }

    public async Task<List<ProductLookupDto>> GetProductsLookup()
    {
        return await
        (
            from product in _context.Products.AsNoTracking()

            join inventory in _context.Inventories
                on product.Id equals inventory.ProductId
                into inventories

            from inventory in inventories.DefaultIfEmpty()

            orderby product.Name

            select new ProductLookupDto
            {
                Id = product.Id,
                Code = product.Code,
                Name = product.Name,

                CurrentStock =
                    inventory == null
                        ? 0
                        : inventory.CurrentQuantity
            }
        ).ToListAsync();
    }

    public async Task<List<ProductLookupDto>> GetOpeningBalanceProductsLookup()
    {
        return await
        (
            from product in _context.Products.AsNoTracking()

            join inventory in _context.Inventories
                on product.Id equals inventory.ProductId
                into inventories

            from inventory in inventories.DefaultIfEmpty()

            where inventory == null
                  || inventory.CurrentQuantity == 0

            orderby product.Name

            select new ProductLookupDto
            {
                Id = product.Id,
                Code = product.Code,
                Name = product.Name,

                CurrentStock =
                    inventory == null
                        ? 0
                        : inventory.CurrentQuantity
            }

        ).ToListAsync();
    }


   
}