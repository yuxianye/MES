using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Solution.Utility.Extensions;

namespace Solution.Desktop.View.Converter
{
    public class EnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum)
            {
                return ((Enum)value).ToDescription();
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
