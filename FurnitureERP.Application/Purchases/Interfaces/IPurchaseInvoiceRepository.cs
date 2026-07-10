using FurnitureERP.Application.Common.Models;
using FurnitureERP.Application.Purchases.DTOs;
//using FurnitureERP.Domain.Entities.Purchases;

namespace FurnitureERP.Application.Purchases.Interfaces;

public interface IPurchaseInvoiceRepository
{
    Task<PurchaseInvoice?> GetEntityById(int id);

    Task<PurchaseInvoiceDetailsDto?> GetById(int id);

    Task<PagedResult<PurchaseInvoiceListDto>> GetAll(
        string search,
        int page,
        int pageSize);

    Task Add(PurchaseInvoice invoice);

    Task Update(PurchaseInvoice invoice);

    Task Delete(PurchaseInvoice invoice);

    Task<bool> InvoiceNumberExists(
        string invoiceNumber);

    Task<string> GenerateNextInvoiceNumber();


    Task<PurchaseInvoicePrintDto?> GetPrintData(int invoiceId);
}