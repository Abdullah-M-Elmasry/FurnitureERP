using FurnitureERP.Application.Common.Models;
using FurnitureERP.Application.Inventory.DTOs;
using FurnitureERP.Application.Products.DTOs;
using FurnitureERP.Domain.Entities.Inventories;
using FurnitureERP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureERP.Application.Inventory.Interfaces;

public interface IInventoryService
{
    Task AddStock(
        int productId,
        decimal quantity,
        InventoryTransactionType transactionType,
        InventoryReferenceType referenceType,
        int? referenceId,
        StockAdjustmentReason? adjustmentReason = null,
        string? notes = null);

    Task RemoveStock(
        int productId,
        decimal quantity,
        InventoryTransactionType transactionType,
        InventoryReferenceType referenceType,
        int? referenceId,
        StockAdjustmentReason? adjustmentReason = null,
        string? notes = null);

    //Task<decimal> GetCurrentBalance(
    //    int productId);

    Task<List<ProductLookupDto>> GetProductsLookup();


    Task<List<ProductLookupDto>> GetOpeningBalanceProductsLookup();

    Task<ProductInventory?> GetInventory(
        int productId);

    Task<PagedResult<InventoryItemDto>> GetAll(
     string search,
     int page,
     int pageSize);

    Task<PagedResult<InventoryTransactionDto>> GetProductTransactions(
        int productId,
        string search,
        int page,
        int pageSize);

    Task<bool> HasEnoughStock(
    int productId,
    decimal quantity);

    Task<decimal> GetAvailableStock(int productId);


    Task ApplyStockAdjustment(StockAdjustmentDto dto);


    Task SetOpeningBalance(
    OpeningBalanceDto dto);
}