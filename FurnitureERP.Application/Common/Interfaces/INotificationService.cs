namespace FurnitureERP.Application.Common.Interfaces
{
    public interface INotificationService
    {
        void Show(string message);

        void ShowSuccess(string message);

        void ShowWarning(string message);
        void ShowError(string message);
    }
}