using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Solution.Desktop.AgvTest.UserControls
{
    public class AgvStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                return Utility.Windows.ResourceHelper.FindResource("AgvStatusColor" + value.ToString());
            }
            return Utility.Windows.ResourceHelper.FindResource("AccentColorBrush");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
