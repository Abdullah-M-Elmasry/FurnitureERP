using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureERP.Application.Common.Interfaces
{
    public interface IDialogRequestClose
    {
        event Action<bool?> RequestClose;
    }
}
