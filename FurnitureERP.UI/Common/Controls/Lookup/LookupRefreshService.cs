namespace FurnitureERP.UI.Common.Controls.Lookup;

public static class LookupRefreshService
{
    public static event Action<Type>? RefreshRequested;

    public static void Publish<T>()
    {
        RefreshRequested?.Invoke(typeof(T));
    }

    public static void Subscribe<T>(Action action)
    {
        RefreshRequested += type =>
        {
            if (type == typeof(T))
                action();
        };
    }
}