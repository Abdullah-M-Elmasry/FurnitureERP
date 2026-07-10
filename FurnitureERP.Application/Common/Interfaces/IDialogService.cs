using FurnitureERP.UI.Common.Dialog;
using System;
using System.Collections.Generic;
using System.Text;
namespace FurnitureERP.Application.Common.Interfaces
{
    public interface IDialogService { 
        bool ShowDialog<TViewModel>(Action<TViewModel>? configure = null)
            where TViewModel : class; 
        TResult? ShowDialog<TViewModel, TResult>(Action<TViewModel>? configure = null) 
            where TViewModel : class, IDialogResult<TResult>;
        Task<bool> Confirm(string message, string title);
    }
}