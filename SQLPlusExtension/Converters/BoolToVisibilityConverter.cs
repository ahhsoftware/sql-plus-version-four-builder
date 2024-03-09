using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SQLPlusExtension.Converters
{
    /// <summary>
    /// Converts true false values to visible values.
    /// If the parameter is supplied, matching the parameter result in visible.
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            bool boolValue = (bool)value;

            if (string.IsNullOrEmpty(parameter as string))
            {
                return boolValue ? Visibility.Visible : Visibility.Collapsed;
            }

            bool parameterValue = bool.Parse(parameter as string);

            if (boolValue)
            {
                return parameterValue ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                return parameterValue ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}