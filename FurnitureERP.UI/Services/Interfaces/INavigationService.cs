using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureERP.UI.Services.Interfaces;

    public interface INavigationService
    {
        Task NavigateTo<TView>(
            object? parameter = null)
            where TView : class;
    }

