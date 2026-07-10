using FurnitureERP.Application.Common.Interfaces;
using MaterialDesignThemes.Wpf;

namespace FurnitureERP.UI.Services
{
    public class NotificationService : INotificationService
    {
        public SnackbarMessageQueue MessageQueue { get; }

        public NotificationService()
        {
            MessageQueue =
                new SnackbarMessageQueue(
                    TimeSpan.FromSeconds(3));
        }

        public void Show(
            string message)
        {
            MessageQueue.Enqueue(message);
        }

        public void ShowSuccess(
            string message)
        {
            MessageQueue.Enqueue(
                $"✓ {message}");
        }

        public void ShowWarning(string message)
        {
            MessageQueue.Enqueue($"⚠ {message}");
        }
        public void ShowError(
            string message)
        {
            MessageQueue.Enqueue(
                $"✕ {message}");
        }
    }
}