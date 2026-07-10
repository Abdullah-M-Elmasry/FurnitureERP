using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace FurnitureERP.UI.Core.Converters
{
    public class HighlightConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string text = values[0]?.ToString() ?? "";
            string search = values[1]?.ToString() ?? "";

            var tb = new TextBlock();

            if (string.IsNullOrWhiteSpace(search))
            {
                tb.Text = text;
                return tb;
            }

            int index = text.IndexOf(search, StringComparison.OrdinalIgnoreCase);

            if (index < 0)
            {
                tb.Text = text;
                return tb;
            }

            tb.Inlines.Add(new Run(text.Substring(0, index)));

            tb.Inlines.Add(new Run(text.Substring(index, search.Length))
            {
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.DarkBlue
            });

            tb.Inlines.Add(new Run(text.Substring(index + search.Length)));

            return tb;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}