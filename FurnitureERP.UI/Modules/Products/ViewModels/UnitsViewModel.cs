using CommunityToolkit.Mvvm.Input;
using FurnitureERP.Application.Common.Interfaces;
using FurnitureERP.Application.Products.Interfaces;
using FurnitureERP.UI.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Text;
using static MaterialDesignThemes.Wpf.Theme.ToolBar;

namespace FurnitureERP.UI.Modules.Products.ViewModels
{
    public partial class UnitsViewModel
    : CrudListViewModel<Unit>
    {
        private readonly IUnitService _unitService;
        private readonly INotificationService _notificationService;
        private readonly IDialogService _dialogService;

        public UnitsViewModel(
            IUnitService unitService,
            INotificationService notificationService,
            IDialogService dialogService)
        {
            _unitService = unitService;
            _notificationService = notificationService;
            _dialogService = dialogService;
        }

        public override async Task Load(bool append = false)
        {
            if (IsLoading)
                return;

            try
            {
                IsLoading = true;

                var result = await _unitService.GetAll(
                    SearchText ?? "",
                    CurrentPage,
                    PageSize);

                if (!append)
                    Items.Clear();

                foreach (var item in result.Items)
                    Items.Add(item);

                TotalPages =
                    (int)Math.Ceiling(result.TotalCount / (double)PageSize);

                HasMoreItems =
                    CurrentPage < TotalPages;
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task OpenAddUnit()
        {
            var saved =
                _dialogService.ShowDialog<AddUnitViewModel>();

            if (saved)
                await Refresh();
        }

        [RelayCommand]
        private async Task EditUnit(Unit unit)
        {
            if (unit == null)
                return;

            var saved =
                _dialogService.ShowDialog<AddUnitViewModel>(
                    vm => vm.SetEntity(unit));

            if (saved)
                await Refresh();
        }

        [RelayCommand]
        private async Task DeleteUnit(Unit unit)
        {
            if (unit == null)
                return;

            if (!await _dialogService.Confirm(
                $"Delete {unit.Name} ?",
                "Confirm Delete"))
                return;

            await _unitService.Delete(unit.Id);

            Items.Remove(unit);

            _notificationService.ShowSuccess(
                "Unit deleted successfully.");
        }
    }
}
