using FurnitureERP.Application.Common.Models;
using FurnitureERP.Application.Sales.DTOs;
using FurnitureERP.Application.Sales.Requests;

namespace FurnitureERP.Application.Sales.Interfaces;

public interface ISalesInvoiceService
{
    Task<int> CreateDraft(CreateSalesInvoiceDraftRequest request);

    Task UpdateDraft(UpdateSalesInvoiceDraftRequest request);

    Task Confirm(ConfirmSalesInvoiceRequest request);

    Task Cancel(CancelSalesInvoiceRequest request);

    Task DeleteDraft(int invoiceId);

    Task<SalesInvoiceDetailsDto?> GetById(int id);

    Task<PagedResult<SalesInvoiceListDto>> GetAll(
        string search,
        int page,
        int pageSize);

    Task<SalesInvoicePrintDto?> GetPrintData(int invoiceId);
}