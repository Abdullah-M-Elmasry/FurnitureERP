using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureERP.Application.Common.Interfaces;

public interface IPdfDocumentService
{
    Task<byte[]> GeneratePurchaseInvoicePdf(int invoiceId);
}
