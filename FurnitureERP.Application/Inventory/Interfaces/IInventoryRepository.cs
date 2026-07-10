using FurnitureERP.Application.Common.Models;
using FurnitureERP.Application.Inventory.DTOs;
using FurnitureERP.Application.Products.DTOs;
using FurnitureERP.Domain.Entities.Inventories;

namespace FurnitureERP.Application.Inventory.Interfaces;

public interface IInventoryRepository
{
    Task<PagedResult<InventoryItemDto>> GetAll(
     string search,
     int page,
     int pageSize);

    Task<PagedResult<InventoryTransactionDto>> GetProductTransactions(
        int productId,
        string search,
        int page,
        int pageSize);

    Task<List<ProductLookupDto>> GetProductsLookup();

    Task<List<ProductLookupDto>> GetOpeningBalanceProductsLookup();

    Task<ProductInventory?> GetByProductId(
        int productId);

    Task<decimal> GetCurrentStock(
        int productId);

    Task<ProductInventory> GetOrCreateInventory(
        int productId);

    Task AddInventory(
        ProductInventory inventory);

    Task UpdateInventory(
        ProductInventory inventory);

    Task AddTransaction(
        InventoryTransaction transaction);

  
}