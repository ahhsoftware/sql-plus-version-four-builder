using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SQLPlusExtension.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //True with parameter true = visible
            //True with parameter false = collapsed

            //False with parameter true = collapsed
            //False with parameter false == visible

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
