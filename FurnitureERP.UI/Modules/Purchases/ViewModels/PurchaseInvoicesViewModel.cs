using CommunityToolkit.Mvvm.Input;
using FurnitureERP.Application.Common.Interfaces;
using FurnitureERP.Application.Purchases.DTOs;
using FurnitureERP.Application.Purchases.Interfaces;
using FurnitureERP.Infrastructure.Documents.Pdf;
using FurnitureERP.UI.Common.ViewModels;
using FurnitureERP.UI.Modules.Purchases.Views;
using FurnitureERP.UI.Services.Interfaces;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using static MaterialDesignThemes.Wpf.Theme.ToolBar;

public partial class PurchaseInvoicesViewModel
    : CrudListViewModel<PurchaseInvoiceListDto>
{
    private readonly IPurchaseInvoiceService _service;

    private readonly INavigationService _navigationService;

    private readonly IPdfDocumentService _pdfService;
    public PurchaseInvoicesViewModel(
     IPurchaseInvoiceService service,
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
       .NavigateTo<PurchaseInvoiceEditorView>();
    }

    [RelayCommand]
    private async Task RefreshInvoices()
    {
        await Refresh();
    }

    [RelayCommand]
    private async Task OpenInvoice(
        PurchaseInvoiceListDto? invoice)
    {
        if (invoice == null)
            return;

        await _navigationService
       .NavigateTo<PurchaseInvoiceEditorView>(invoice.Id);

       
    }

    [RelayCommand]
    private async Task EditInvoice(PurchaseInvoiceListDto? invoice)
    {
        if (invoice == null)
            return;

        await _navigationService
            .NavigateTo<PurchaseInvoiceEditorView>(invoice.Id);
    }

    [RelayCommand]
    private async Task ViewInvoice(PurchaseInvoiceListDto? invoice)
    {
        if (invoice == null)
            return;

        await _navigationService
            .NavigateTo<PurchaseInvoiceEditorView>(invoice.Id);
    }


    [RelayCommand]
    private async Task PrintInvoice(
        PurchaseInvoiceListDto? invoice)
    {
        if (invoice == null)
            return;

        var pdf =
            await _pdfService
                .GeneratePurchaseInvoicePdf(invoice.Id);

        var path = Path.Combine(
            Environment.GetFolderPath(
                Environment.SpecialFolder.Desktop),
            $"PurchaseInvoice-{invoice.InvoiceNumber}.pdf");

        await File.WriteAllBytesAsync(path, pdf);

        Process.Start(new ProcessStartInfo(path)
        {
            UseShellExecute = true
        });
    }
}