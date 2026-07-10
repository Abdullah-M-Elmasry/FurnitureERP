namespace FurnitureERP.UI.Common.Controls.Lookup;

// 🔥 OPTIONAL: helper لو عايز تعمل projection قبل العرض
public sealed class LookupItemViewModel
{
    public object Item { get; init; } = default!;

    public string Code { get; init; } = "";

    public string Name { get; init; } = "";

    // 🔥 unified search text
    public string SearchText =>
        $"{Code} {Name}";
}