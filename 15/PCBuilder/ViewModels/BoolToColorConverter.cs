using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace PCBuilder.ViewModels;

public class BoolToColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool b)
            return b ? new SolidColorBrush(Color.Parse("#44FF88")) : new SolidColorBrush(Color.Parse("#FF8844"));
        return new SolidColorBrush(Colors.White);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
