using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using FurnitureERP.Domain.Enums;

namespace FurnitureERP.UI.Converters;


public class InvoiceStatusColorConverter : IValueConverter
{
    public object Convert(object value,
        Type targetType,
        object parameter,
        CultureInfo culture)
    {
        var status = value?.ToString();

        return status switch
        {
            "Draft" => Brushes.DarkOrange,

            "Confirmed" => Brushes.ForestGreen,

            "Cancelled" => Brushes.Firebrick,

            _ => Brushes.Gray
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}