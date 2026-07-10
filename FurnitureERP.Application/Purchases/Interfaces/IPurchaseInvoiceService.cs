using FurnitureERP.Application.Common.Models;
using FurnitureERP.Application.Purchases.DTOs;
using FurnitureERP.Application.Purchases.Requests;

namespace FurnitureERP.Application.Purchases.Interfaces;

public interface IPurchaseInvoiceService
{
    Task<int> CreateDraft(
        CreatePurchaseInvoiceDraftRequest request);

    Task UpdateDraft(
        UpdatePurchaseInvoiceDraftRequest request);

    Task Confirm(
        ConfirmPurchaseInvoiceRequest request);

    Task Cancel(
        CancelPurchaseInvoiceRequest request);

    Task DeleteDraft(
        int invoiceId);

    Task<PurchaseInvoiceDetailsDto?> GetById(int id);

    Task<PagedResult<PurchaseInvoiceListDto>> GetAll(
        string search,
        int page,
        int pageSize);



    Task<PurchaseInvoicePrintDto?> GetPrintData(int invoiceId);
}