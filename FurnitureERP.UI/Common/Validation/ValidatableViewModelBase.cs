using CommunityToolkit.Mvvm.ComponentModel;

namespace FurnitureERP.UI.Common.Validation;

public abstract class ValidatableViewModelBase
    : ObservableValidator
{
    protected virtual bool Validate()
    {
        ValidateAllProperties();

        return !HasErrors;
    }
}