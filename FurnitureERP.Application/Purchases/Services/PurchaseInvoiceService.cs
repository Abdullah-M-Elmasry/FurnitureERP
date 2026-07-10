using FurnitureERP.Application.Common.Interfaces;
using FurnitureERP.Application.Common.Models;
using FurnitureERP.Application.Inventory.Interfaces;
using FurnitureERP.Application.Purchases.DTOs;
using FurnitureERP.Application.Purchases.Interfaces;
using FurnitureERP.Application.Purchases.Requests;
//using FurnitureERP.Domain.Entities.Purchases;
using FurnitureERP.Domain.Enums;

namespace FurnitureERP.Application.Purchases.Services;

public class PurchaseInvoiceService
    : IPurchaseInvoiceService
{
    private readonly IPurchaseInvoiceRepository _repository;
    private readonly IInventoryService _inventoryService;
    private readonly IUnitOfWork _unitOfWork;

    public PurchaseInvoiceService(
        IPurchaseInvoiceRepository repository,
        IInventoryService inventoryService,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _inventoryService = inventoryService;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> CreateDraft(
     CreatePurchaseInvoiceDraftRequest request)
    {
        
            var invoice = new PurchaseInvoice
        {
            InvoiceNumber =
                await _repository.GenerateNextInvoiceNumber(),

            Date = request.Date,
            DueDate = request.DueDate,

            SupplierId = request.SupplierId,

            Notes = request.Notes,

            Discount = request.Discount,
            Tax = request.Tax,

            Status = PurchaseInvoiceStatus.Draft,

            CreatedAt = DateTime.UtcNow
        };

        foreach (var item in request.Items)
        {
            invoice.Items.Add(
                new PurchaseInvoiceItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    CostPrice = item.CostPrice
                });
        }

        CalculateTotals(invoice);
        

        try
        {
            await _unitOfWork.BeginTransactionAsync();

            await _repository.Add(invoice);

            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.CommitTransactionAsync();

            return invoice.Id;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    private static void CalculateTotals(
    PurchaseInvoice invoice)
    {
        invoice.SubTotal =
            invoice.Items.Sum(x =>
                x.Quantity * x.CostPrice);

        invoice.GrandTotal =
            invoice.SubTotal +
            invoice.Tax -
            invoice.Discount;
    }

    public async Task UpdateDraft(
    UpdatePurchaseInvoiceDraftRequest request)
    {
        var invoice =
            await _repository.GetEntityById(
                request.Id);

        if (invoice == null)
            throw new Exception(
                "Invoice not found");

        if (invoice.Status !=
            PurchaseInvoiceStatus.Draft)
        {
            throw new Exception(
                "Only draft invoices can be edited");
        }

        invoice.Date = request.Date;

        invoice.DueDate = request.DueDate;

        invoice.SupplierId = request.SupplierId;

        invoice.Notes = request.Notes;

        invoice.Discount = request.Discount;

        invoice.Tax = request.Tax;

        invoice.LastModifiedAt =
            DateTime.UtcNow;

        invoice.Items.Clear();

        foreach (var item in request.Items)
        {
            invoice.Items.Add(
                new PurchaseInvoiceItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    CostPrice = item.CostPrice
                });
        }

        CalculateTotals(invoice);
       
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            await _repository.Update(invoice);

            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.CommitTransactionAsync();
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task Confirm(
    ConfirmPurchaseInvoiceRequest request)
    {

        var invoice =
        await _repository.GetEntityById(
            request.InvoiceId);


        if (invoice == null)
            throw new Exception(
                "Invoice not found");

        if (invoice.Status !=
            PurchaseInvoiceStatus.Draft)
        {
            throw new Exception(
                "Invoice already confirmed");
        }
       
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            foreach (var item in invoice.Items)
            {
                await _inventoryService.AddStock(
                    item.ProductId,
                    item.Quantity,
                    InventoryTransactionType.Purchase,
                    InventoryReferenceType.PurchaseInvoice,

                    invoice.Id,
                    null,
                    $"Purchase Invoice {invoice.InvoiceNumber}");
            }

            invoice.Status =
                PurchaseInvoiceStatus.Confirmed;

            invoice.ConfirmedAt =
                DateTime.UtcNow;

            invoice.ConfirmedBy =
                request.ConfirmedBy;

            await _repository.Update(invoice);

            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.CommitTransactionAsync();
        }

        catch
        {
            await _unitOfWork.RollbackTransactionAsync();

            throw;
        }
    }

    public async Task Cancel(
    CancelPurchaseInvoiceRequest request)
    {
        var invoice =
            await _repository.GetEntityById(
                request.InvoiceId);

        if (invoice == null)
            throw new Exception(
                "Invoice not found");

        if (invoice.Status ==
            PurchaseInvoiceStatus.Cancelled)
        {
            return;
        }

        if (invoice.Status ==
            PurchaseInvoiceStatus.Confirmed)
        {
            throw new Exception(
                "Confirmed invoice cannot be cancelled");
        }

        invoice.Status =
            PurchaseInvoiceStatus.Cancelled;

        invoice.LastModifiedAt =
            DateTime.UtcNow;

       
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            await _repository.Update(invoice);

            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.CommitTransactionAsync();
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task DeleteDraft(
    int invoiceId)
    {
        var invoice =
            await _repository.GetEntityById(
                invoiceId);

        if (invoice == null)
            throw new Exception("Invoice not found");

        if (invoice.Status !=
            PurchaseInvoiceStatus.Draft)
        {
            throw new Exception(
                "Only draft invoices can be deleted");
        }

       
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            await _repository.Delete(invoice);

            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.CommitTransactionAsync();
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public Task<PurchaseInvoiceDetailsDto?> GetById(int id)
    {
        return _repository.GetById(id);
    }

    public Task<PagedResult<PurchaseInvoiceListDto>> GetAll(
        string search,
        int page,
        int pageSize)
    {
        return _repository.GetAll(
            search,
            page,
            pageSize);
    }

    public Task<PurchaseInvoicePrintDto?> GetPrintData(
    int invoiceId)
    {
        return _repository.GetPrintData(invoiceId);
    }

}