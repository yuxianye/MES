using Solution.Desktop.Core;
using Solution.Desktop.MatWareHouseInfo.Model;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Solution.Desktop.MatWareHouseAreaInfo.View
{
    /// <summary>
    /// WareHouseInfoAddView.xaml 的交互逻辑
    /// </summary>
    public partial class MatWareHouseAreaInfoAddView : UserControlBase
    {
        public MatWareHouseAreaInfoAddView()
        {
            InitializeComponent();
            firstIC.Focus();
        }

        private void LayerNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9]+");
            e.Handled = re.IsMatch(e.Text);
        }

        private void LayerNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            NumberUtility.isIntegerNumber(sender, 10);
        }

        private void ColumnNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9]+");
            e.Handled = re.IsMatch(e.Text);
        }

        private void ColumnNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            NumberUtility.isIntegerNumber(sender, 20);
        }


        private void LocationLoadBearing_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9.-]+");
            e.Handled = re.IsMatch(e.Text);
        }

        private void LocationLoadBearing_TextChanged(object sender, TextChangedEventArgs e)
        {
            NumberUtility.isDecimalNumber(sender, 1000);
        }
    }
}
