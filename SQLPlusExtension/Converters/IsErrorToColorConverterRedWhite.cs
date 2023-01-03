using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace SQLPlusExtension.Converters
{
    public class IsErrorToColorConverterRedWhite : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            SolidColorBrush result = new SolidColorBrush(System.Windows.Media.Colors.White);

            if ((bool)value)
            {
                {
                    result = new SolidColorBrush(System.Windows.Media.Colors.Red);
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}