using FurnitureERP.Application.Common.Interfaces;
using FurnitureERP.Application.Common.Models;
using FurnitureERP.Application.Inventory.DTOs;
using FurnitureERP.Application.Inventory.Interfaces;
using FurnitureERP.Application.Products.DTOs;
using FurnitureERP.Domain.Entities.Inventories;
using FurnitureERP.Domain.Enums;

namespace FurnitureERP.Infrastructure.Inventory.Services;

public class InventoryService : IInventoryService
{


    private readonly IInventoryRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public InventoryService(
        IInventoryRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }


    //public async Task<decimal> GetCurrentBalance(
    //int productId)
    //{
    //    var inventory =
    //        await _repository.GetByProductId(
    //            productId);

    //    return inventory?.CurrentQuantity ?? 0;
    //}

    public Task<ProductInventory?> GetInventory(
    int productId)
    {
        return _repository.GetByProductId(
            productId);
    }

    public Task<PagedResult<InventoryItemDto>> GetAll(
    string search,
    int page,
    int pageSize)
    {
        return _repository.GetAll(
            search,
            page,
            pageSize);
    }

    public Task<PagedResult<InventoryTransactionDto>> GetProductTransactions(
        int productId,
        string search,
        int page,
        int pageSize)
    {
        return _repository.GetProductTransactions(
            productId,
            search,
            page,
            pageSize);
    }

    public Task<List<ProductLookupDto>> GetProductsLookup()
    {
        return _repository.GetProductsLookup();
    }

    public Task<List<ProductLookupDto>> GetOpeningBalanceProductsLookup()
    {
        return _repository.GetOpeningBalanceProductsLookup();
    }

    public async Task AddStock(
    int productId,
    decimal quantity,
    InventoryTransactionType transactionType,
    InventoryReferenceType referenceType,
    int? referenceId = null,
    StockAdjustmentReason? adjustmentReason = null,
    string? notes = null)
    {
        if (quantity <= 0)
            throw new Exception("Quantity must be greater than zero");

        var inventory =
            await _repository.GetOrCreateInventory(
                productId);

        inventory.CurrentQuantity += quantity;

        inventory.UpdatedAt = DateTime.Now;

        //await _repository.UpdateInventory(  //رجعته تاني 
        //    inventory);

        await _repository.AddTransaction(
            new InventoryTransaction
            {
                ProductId = productId,
                Quantity = quantity,
                Type = transactionType,
                TransactionDate = DateTime.Now,
                ReferenceId = referenceId,
                ReferenceType = referenceType,
                Notes = notes,
                BalanceAfter = inventory.CurrentQuantity,
                AdjustmentReason = adjustmentReason,

            });

        //await _repository.SaveChanges();
    }

    public async Task RemoveStock(
    int productId,
    decimal quantity,
     InventoryTransactionType transactionType,
    InventoryReferenceType referenceType,
    int? referenceId = null,
    StockAdjustmentReason? adjustmentReason = null,
    string? notes = null)
    {
        var stock = await GetAvailableStock(productId);

        if (stock < quantity)
            throw new Exception(
                "Insufficient stock");

        if (quantity <= 0)
            throw new Exception("Quantity must be greater than zero");

        var inventory =
            await _repository.GetOrCreateInventory(
                productId);

        if (inventory.CurrentQuantity < quantity)
            throw new Exception("Insufficient stock");

        inventory.CurrentQuantity -= quantity;

        inventory.UpdatedAt = DateTime.Now;

        //await _repository.UpdateInventory(
        //    inventory);

        await _repository.AddTransaction(
            new InventoryTransaction
            {
                ProductId = productId,
                Quantity = quantity,
                Type = transactionType,
                TransactionDate = DateTime.Now,
                ReferenceId = referenceId,
                ReferenceType = referenceType,
                Notes = notes,
                BalanceAfter = inventory.CurrentQuantity,
                AdjustmentReason = adjustmentReason,
                
            });

        //await _repository.SaveChanges();
    }

    public async Task<bool> HasEnoughStock(
    int productId,
    decimal quantity)
    {
        var balance =
            await GetAvailableStock(productId);

        return balance >= quantity;
    }


    public async Task<decimal> GetAvailableStock(int productId)
    {
        return await _repository.GetCurrentStock(productId);
    }


    public async Task ApplyStockAdjustment(StockAdjustmentDto dto)
    {
        if (dto.Quantity <= 0)
            throw new Exception("Quantity must be greater than zero.");

        switch (dto.Reason)
        {
            case StockAdjustmentReason.InventoryCountIncrease:
            case StockAdjustmentReason.Found:
            case StockAdjustmentReason.ProductionOutput:
            case StockAdjustmentReason.ManualIncrease:

                await AddStock(
                    dto.ProductId,
                    dto.Quantity,
                    InventoryTransactionType.Adjustment,
                    InventoryReferenceType.Adjustment,
                    null,
                    dto.Reason,
                    dto.Notes);

                break;

            case StockAdjustmentReason.InventoryCountDecrease:
            case StockAdjustmentReason.Damaged:
            case StockAdjustmentReason.Lost:
            case StockAdjustmentReason.ProductionConsumption:
            case StockAdjustmentReason.ManualDecrease:

                await RemoveStock(
                    dto.ProductId,
                    dto.Quantity,
                    InventoryTransactionType.Adjustment,
                    InventoryReferenceType.Adjustment,
                    null,
                    dto.Reason,
                    dto.Notes);

                break;

            default:
                throw new Exception("Unsupported stock adjustment reason.");
        }

        await _unitOfWork.SaveChangesAsync();
    }


    public async Task SetOpeningBalance(
     OpeningBalanceDto dto)
    {
        if (dto.Quantity < 0)
            throw new Exception("Opening balance cannot be negative.");

        var inventory =
            await _repository.GetOrCreateInventory(dto.ProductId);

        if (inventory.CurrentQuantity != 0)
            throw new Exception("Opening balance has already been set.");

        inventory.CurrentQuantity = dto.Quantity;
        inventory.UpdatedAt = DateTime.Now;

        await _repository.AddTransaction(
            new InventoryTransaction
            {
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                Type = InventoryTransactionType.OpeningBalance,
                TransactionDate = DateTime.Now,
                Notes = dto.Notes,
                BalanceAfter = dto.Quantity
            });


        await _unitOfWork.SaveChangesAsync();
    }


}