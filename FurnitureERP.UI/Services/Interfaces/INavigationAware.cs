using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureERP.UI.Services.Interfaces;

    public interface INavigationAware
    {
        Task OnNavigatedTo(object? parameter);
    }

