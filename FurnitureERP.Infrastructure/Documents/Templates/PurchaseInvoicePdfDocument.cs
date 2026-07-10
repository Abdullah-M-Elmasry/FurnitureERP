using FurnitureERP.Application.Purchases.DTOs;
using FurnitureERP.Infrastructure.Documents.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace FurnitureERP.Infrastructure.Documents.Templates;

public class PurchaseInvoicePdfDocument
    : ERPDocumentBase
{
    private readonly PurchaseInvoicePrintDto _invoice;

    private readonly CompanyInfo _company;

    public PurchaseInvoicePdfDocument(
        PurchaseInvoicePrintDto invoice,
        CompanyInfo company)
    {
        _invoice = invoice;
        _company = company;
    }

    public override void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4);

            page.Margin(25);

            page.DefaultTextStyle(x =>
                x.FontSize(11));

            page.Content().Column(column =>
            {
                DrawHeader(column);

                DrawSupplierInfo(column);

                DrawItemsTable(column);

                DrawTotals(column);

                DrawFooter(column);
            });
        });
    }


    private void DrawHeader(ColumnDescriptor column)
    {
        column.Item().Row(row =>
        {
            row.RelativeItem().Column(col =>
            {
                col.Item().Text(_company.Name)
                    .FontSize(20)
                    .Bold();

                col.Item().Text(_company.Address);

                col.Item().Text($"Phone : {_company.Phone}");
            });

            row.ConstantItem(180).Column(col =>
            {
                col.Item()
                    .AlignRight()
                    .Text("PURCHASE INVOICE")
                    .FontSize(22)
                    .Bold();

                col.Item()
                    .AlignRight()
                    .Text($"No : {_invoice.InvoiceNumber}");

                col.Item()
                    .AlignRight()
                    .Text($"Date : {_invoice.Date:dd/MM/yyyy}");
            });
        });

        column.Item().PaddingVertical(15).LineHorizontal(1);
    }

    private void DrawSupplierInfo(ColumnDescriptor column)
    {
        column.Item().Text("Supplier")
            .Bold()
            .FontSize(14);

        column.Item().Text(_invoice.SupplierName);

        column.Item().PaddingBottom(15);
    }

    private void DrawItemsTable(ColumnDescriptor column)
    {
        column.Item().Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(45);   // #
                columns.RelativeColumn();     // Product
                columns.ConstantColumn(70);   // Qty
                columns.ConstantColumn(90);   // Price
                columns.ConstantColumn(100);  // Total
            });

            // Header
            table.Header(header =>
            {
                static IContainer Cell(IContainer c) =>
                    c.BorderBottom(1)
                     .PaddingVertical(6);

                header.Cell().Element(Cell).Text("#").Bold();

                header.Cell().Element(Cell).Text("Product").Bold();

                header.Cell().Element(Cell).AlignCenter().Text("Qty").Bold();

                header.Cell().Element(Cell).AlignRight().Text("Price").Bold();

                header.Cell().Element(Cell).AlignRight().Text("Total").Bold();
            });

            int index = 1;

            foreach (var item in _invoice.Items)
            {
                table.Cell().PaddingVertical(5).Text(index++.ToString());

                table.Cell().PaddingVertical(5).Text(item.Name);

                table.Cell().PaddingVertical(5)
                    .AlignCenter()
                    .Text(item.Quantity.ToString());

                table.Cell().PaddingVertical(5)
                    .AlignRight()
                    .Text(item.CostPrice.ToString("N2"));

                table.Cell().PaddingVertical(5)
                    .AlignRight()
                    .Text(item.Total.ToString("N2"));
            }
        });

        column.Item().PaddingBottom(20);
    }

    private void DrawTotals(ColumnDescriptor column)
    {
        column.Item().AlignRight().Width(250).Column(col =>
        {
            col.Item().Row(row =>
            {
                row.RelativeItem().Text("Sub Total")
                    .SemiBold();

                row.ConstantItem(100)
                    .AlignRight()
                    .Text(_invoice.SubTotal.ToString("N2"));
            });

            col.Item().Row(row =>
            {
                row.RelativeItem().Text("Discount");

                row.ConstantItem(100)
                    .AlignRight()
                    .Text(_invoice.Discount.ToString("N2"));
            });

            col.Item().Row(row =>
            {
                row.RelativeItem().Text("Tax");

                row.ConstantItem(100)
                    .AlignRight()
                    .Text(_invoice.Tax.ToString("N2"));
            });

            col.Item()
                .PaddingVertical(6)
                .LineHorizontal(1);

            col.Item().Row(row =>
            {
                row.RelativeItem()
                    .Text("Grand Total")
                    .Bold()
                    .FontSize(14);

                row.ConstantItem(100)
                    .AlignRight()
                    .Text(_invoice.GrandTotal.ToString("N2"))
                    .Bold()
                    .FontSize(14);
            });
        });

        column.Item().PaddingBottom(20);
    }

    private void DrawFooter(ColumnDescriptor column)
    {
        column.Item().PaddingTop(30);

        if (!string.IsNullOrWhiteSpace(_invoice.Notes))
        {
            column.Item()
                .Text("Notes")
                .Bold();

            column.Item()
                .Text(_invoice.Notes);

            column.Item().PaddingBottom(20);
        }

        column.Item()
            .AlignCenter()
            .Text(text =>
            {
                text.Span("Generated by Furniture ERP")
                    .FontSize(10)
                    .FontColor(Colors.Grey.Medium);
            });
    }
}