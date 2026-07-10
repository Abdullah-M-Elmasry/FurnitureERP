using FurnitureERP.Application.Common.Interfaces;
using FurnitureERP.Application.Common.Models;
using FurnitureERP.Application.Inventory.Interfaces;
using FurnitureERP.Application.Sales.DTOs;
using FurnitureERP.Application.Sales.Interfaces;
using FurnitureERP.Application.Sales.Requests;
using FurnitureERP.Domain.Entities.Sales;
using FurnitureERP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureERP.Application.Sales.Services
{
    public class SalesInvoiceService : ISalesInvoiceService
    {
        private readonly ISalesInvoiceRepository _repository;
        private readonly IInventoryService _inventoryService;
        private readonly IUnitOfWork _unitOfWork;

        public SalesInvoiceService(
            ISalesInvoiceRepository repository,
            IInventoryService inventoryService,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _inventoryService = inventoryService;
            _unitOfWork = unitOfWork;
        }




        private static void CalculateTotals(
    SalesInvoice invoice)
        {
            invoice.SubTotal =
                invoice.Items.Sum(x =>
                    x.Quantity * x.SalePrice);

            invoice.GrandTotal =
                invoice.SubTotal +
                invoice.Tax -
                invoice.Discount;

            //invoice.RemainingAmount =
            //    invoice.GrandTotal -
            //    invoice.PaidAmount;
        }
        public Task Cancel(CancelSalesInvoiceRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task Confirm(
    ConfirmSalesInvoiceRequest request)
        {
            var invoice =
                await _repository.GetEntityById(
                    request.InvoiceId);

            if (invoice == null)
                throw new Exception("Invoice not found");

            if (invoice.Status != SalesInvoiceStatus.Draft)
                throw new Exception("Invoice already confirmed");

            // نتأكد من الرصيد الأول
            foreach (var item in invoice.Items)
            {
                var hasEnough =
                    await _inventoryService.HasEnoughStock(
                        item.ProductId,
                        item.Quantity);

                if (!hasEnough)
                {
                    throw new Exception(
                        $"Product '{item.ProductName}' does not have enough stock.");
                }
            }

            if (invoice.PaidAmount > invoice.GrandTotal)
                throw new Exception(
                    "Paid amount cannot exceed invoice total.");

            if (invoice.Items.Count == 0)
                throw new Exception("Invoice has no items.");
            try
            {
                

                await _unitOfWork.BeginTransactionAsync();


                foreach (var item in invoice.Items)
                {
                    await _inventoryService.RemoveStock(
                        item.ProductId,
                        item.Quantity,
                        InventoryTransactionType.Sale,
                        InventoryReferenceType.SalesInvoice,
                        invoice.Id,
                        null,
                        $"Sales Invoice {invoice.InvoiceNumber}");
                }

                invoice.Status =
                    SalesInvoiceStatus.Confirmed;

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

        public async Task<int> CreateDraft(
    CreateSalesInvoiceDraftRequest request)
        {
            var invoice = new SalesInvoice
            {
                InvoiceNumber =
                    await _repository.GenerateNextInvoiceNumber(),

                Date = request.Date,

                DueDate = request.DueDate,

                CustomerId = request.CustomerId,

                Notes = request.Notes,

                Discount = request.Discount,

                Tax = request.Tax,

                PaidAmount = request.PaidAmount,

                Status = SalesInvoiceStatus.Draft,

                CreatedAt = DateTime.UtcNow
            };

            foreach (var item in request.Items)
            {
                invoice.Items.Add(
                    new SalesInvoiceItem
                    {
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Quantity = item.Quantity,
                        SalePrice = item.SalePrice
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

        public async Task DeleteDraft(
     int invoiceId)
        {
            var invoice =
                await _repository.GetEntityById(invoiceId);

            if (invoice == null)
                throw new Exception("Invoice not found");

            if (invoice.Status != SalesInvoiceStatus.Draft)
                throw new Exception(
                    "Only draft invoices can be deleted");

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
        public Task<SalesInvoiceDetailsDto?> GetById(int id)
        {
            return _repository.GetById(id);
        }

        public Task<PagedResult<SalesInvoiceListDto>> GetAll(
            string search,
            int page,
            int pageSize)
        {
            return _repository.GetAll(
                search,
                page,
                pageSize);
        }

        public Task<SalesInvoicePrintDto?> GetPrintData(
            int invoiceId)
        {
            return _repository.GetPrintData(invoiceId);
        }
        public async Task UpdateDraft(
    UpdateSalesInvoiceDraftRequest request)
        {
            var invoice =
                await _repository.GetEntityById(request.Id);

            if (invoice == null)
                throw new Exception("Invoice not found");

            if (invoice.Status != SalesInvoiceStatus.Draft)
                throw new Exception("Only draft invoices can be edited");

            invoice.Date = request.Date;

            invoice.DueDate = request.DueDate;

            invoice.CustomerId = request.CustomerId;

            invoice.Notes = request.Notes;

            invoice.Discount = request.Discount;

            invoice.Tax = request.Tax;

            invoice.PaidAmount = request.PaidAmount;

            invoice.LastModifiedAt = DateTime.UtcNow;

            invoice.Items.Clear();

            foreach (var item in request.Items)
            {
                invoice.Items.Add(
                    new SalesInvoiceItem
                    {
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Quantity = item.Quantity,
                        SalePrice = item.SalePrice
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
    }
}
