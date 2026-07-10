using FurnitureERP.Application.Common.Interfaces;
using FurnitureERP.UI.Common.Validation;

namespace FurnitureERP.UI.Common.Dialog;

public abstract class DialogViewModelBase
    : ValidatableViewModelBase,
      IDialogRequestClose
{
    public event Action<bool?>? RequestClose;

    protected void Close(bool result)
    {
        RequestClose?.Invoke(result);
    }
}