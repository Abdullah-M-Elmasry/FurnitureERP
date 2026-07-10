using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace FurnitureERP.UI.Common.Controls.Lookup;

internal static class PropertyAccessorCache
{
    private static readonly ConcurrentDictionary<string,
        Func<object, string>> _cache = new();

    public static string GetValue(
        object item,
        string propertyName)
    {
        var key =
            $"{item.GetType().FullName}.{propertyName}";

        var accessor =
            _cache.GetOrAdd(
                key,
                _ => CreateAccessor(
                    item.GetType(),
                    propertyName));

        return accessor(item);
    }

    private static Func<object, string> CreateAccessor(
        Type type,
        string propertyName)
    {
        var property =
            type.GetProperty(
                propertyName,
                BindingFlags.Instance |
                BindingFlags.Public);

        if (property == null)
            return _ => "";

        var parameter =
            Expression.Parameter(typeof(object));

        var cast =
            Expression.Convert(parameter, type);

        var propertyAccess =
            Expression.Property(cast, property);

        var convert =
            Expression.Call(
                propertyAccess,
                nameof(object.ToString),
                Type.EmptyTypes);

        var nullCheck =
            Expression.Condition(
                Expression.Equal(
                    propertyAccess,
                    Expression.Constant(null)),
                Expression.Constant(""),
                convert);

        var lambda =
            Expression.Lambda<Func<object, string>>(
                nullCheck,
                parameter);

        return lambda.Compile();
    }
}