namespace FurnitureERP.UI.Common.Dialog;

public interface IDialogResult<out TResult>
{
    TResult? DialogResult { get; }
}