using System.Collections;

namespace FurnitureERP.UI.Common.Controls.Lookup;

internal static class LookupSearchEngine
{
    public static IEnumerable<object> Filter(
        IEnumerable? items,
        string? searchText,
        string searchMemberPaths)
    {
        if (items == null)
            yield break;

        searchText = searchText?
            .Trim()
            .ToLower() ?? "";

        var fields =
            searchMemberPaths
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .ToArray();

        foreach (var item in items)
        {
            if (item == null)
                continue;

            if (IsMatch(item, searchText, fields))
                yield return item;
        }
    }

    private static bool IsMatch(
        object item,
        string search,
        string[] fields)
    {
        if (string.IsNullOrWhiteSpace(search))
            return true;

        foreach (var field in fields)
        {
            var value =
    PropertyAccessorCache
        .GetValue(item, field)
        .ToLower();

            if (!string.IsNullOrWhiteSpace(value)
                && value.Contains(search))
            {
                return true;
            }
        }

        return false;
    }
}