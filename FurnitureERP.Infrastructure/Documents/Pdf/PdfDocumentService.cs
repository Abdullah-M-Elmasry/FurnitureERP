using FurnitureERP.Application.Common.Interfaces;
using FurnitureERP.Application.Purchases.Interfaces;
using FurnitureERP.Infrastructure.Documents.Models;
using FurnitureERP.Infrastructure.Documents.Templates;

namespace FurnitureERP.Infrastructure.Documents.Pdf;

public class PdfDocumentService : IPdfDocumentService
{
    private readonly IPurchaseInvoiceService _purchaseInvoiceService;

    public PdfDocumentService(
        IPurchaseInvoiceService purchaseInvoiceService)
    {
        _purchaseInvoiceService = purchaseInvoiceService;
    }

    public async Task<byte[]> GeneratePurchaseInvoicePdf(int invoiceId)
    {
        var dto =
            await _purchaseInvoiceService.GetPrintData(invoiceId);

        if (dto == null)
            throw new Exception("Invoice not found");

        var company =
        new CompanyInfo();

        var document =
            new PurchaseInvoicePdfDocument(
                dto,
                company);

        return document.GeneratePdf();
    }
}