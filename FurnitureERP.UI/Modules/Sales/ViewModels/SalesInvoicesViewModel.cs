using CommunityToolkit.Mvvm.Input;
using FurnitureERP.Application.Common.Interfaces;
using FurnitureERP.Application.Sales.DTOs;
using FurnitureERP.Application.Sales.Interfaces;
using FurnitureERP.UI.Common.ViewModels;
using FurnitureERP.UI.Modules.Sales.Views;
using FurnitureERP.UI.Services.Interfaces;
using System.Diagnostics;
using System.IO;

namespace FurnitureERP.UI.Modules.Sales.ViewModels;

public partial class SalesInvoicesViewModel
    : CrudListViewModel<SalesInvoiceListDto>
{
    private readonly ISalesInvoiceService _service;

    private readonly INavigationService _navigationService;

    private readonly IPdfDocumentService _pdfService;

    public SalesInvoicesViewModel(
        ISalesInvoiceService service,
        INavigationService navigationService,
        IPdfDocumentService pdfService)
    {
        _service = service;
        _navigationService = navigationService;
        _pdfService = pdfService;
    }

    public override async Task Load(bool append = false)
    {
        if (IsLoading)
            return;

        try
        {
            IsLoading = true;

            var result =
                await _service.GetAll(
                    SearchText ?? "",
                    CurrentPage,
                    PageSize);

            if (!append)
                Items.Clear();

            foreach (var item in result.Items)
            {
                Items.Add(item);
            }

            TotalPages =
                (int)Math.Ceiling(
                    result.TotalCount /
                    (double)PageSize);

            HasMoreItems =
                CurrentPage < TotalPages;
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task NewInvoice()
    {
        await _navigationService
            .NavigateTo<SalesInvoiceEditorView>();
    }

    [RelayCommand]
    private async Task RefreshInvoices()
    {
        await Refresh();
    }

    [RelayCommand]
    private async Task OpenInvoice(
        SalesInvoiceListDto? invoice)
    {
        if (invoice == null)
            return;

        await _navigationService
            .NavigateTo<SalesInvoiceEditorView>(
                invoice.Id);
    }

    [RelayCommand]
    private async Task EditInvoice(
        SalesInvoiceListDto? invoice)
    {
        if (invoice == null)
            return;

        await _navigationService
            .NavigateTo<SalesInvoiceEditorView>(
                invoice.Id);
    }

    [RelayCommand]
    private async Task ViewInvoice(
        SalesInvoiceListDto? invoice)
    {
        if (invoice == null)
            return;

        await _navigationService
            .NavigateTo<SalesInvoiceEditorView>(
                invoice.Id);
    }

    [RelayCommand]
    private async Task PrintInvoice(
    SalesInvoiceListDto? invoice)
    {
        //if (invoice == null)
        //    return;

        //var pdf =
        //    await _pdfService
        //        .GenerateSalesInvoicePdf(invoice.Id);

        //var path = Path.Combine(
        //    Environment.GetFolderPath(
        //        Environment.SpecialFolder.Desktop),
        //    $"SalesInvoice-{invoice.InvoiceNumber}.pdf");

        //await File.WriteAllBytesAsync(
        //    path,
        //    pdf);

        //Process.Start(
        //    new ProcessStartInfo(path)
        //    {
        //        UseShellExecute = true
        //    });
    }
}