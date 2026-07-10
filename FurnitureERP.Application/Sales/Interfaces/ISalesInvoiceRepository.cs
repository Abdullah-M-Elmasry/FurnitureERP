using FurnitureERP.Application.Common.Models;
using FurnitureERP.Application.Sales.DTOs;
using FurnitureERP.Domain.Entities.Sales;

namespace FurnitureERP.Application.Sales.Interfaces;

public interface ISalesInvoiceRepository
{
    Task<SalesInvoice?> GetEntityById(int id);

    Task<SalesInvoiceDetailsDto?> GetById(int id);

    Task<PagedResult<SalesInvoiceListDto>> GetAll(
        string search,
        int page,
        int pageSize);

    Task Add(SalesInvoice invoice);

    Task Update(SalesInvoice invoice);

    Task Delete(SalesInvoice invoice);

    Task<bool> InvoiceNumberExists(string invoiceNumber);

    Task<string> GenerateNextInvoiceNumber();


    Task<SalesInvoicePrintDto?> GetPrintData(int invoiceId);
}