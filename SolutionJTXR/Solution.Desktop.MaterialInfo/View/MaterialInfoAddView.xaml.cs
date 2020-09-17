using Solution.Desktop.Core;
using Solution.Desktop.MatWareHouseInfo.Model;
using System.Text.RegularExpressions;

namespace Solution.Desktop.MaterialInfo.View
{
    /// <summary>
    /// WareHouseInfoAddView.xaml 的交互逻辑
    /// </summary>
    public partial class MaterialInfoAddView : UserControlBase
    {
        public MaterialInfoAddView()
        {
            InitializeComponent();
            firstIC.Focus();
        }

        private void FullPalletQuantity_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9]+");
            e.Handled = re.IsMatch(e.Text);
        }

        private void FullPalletQuantity_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            NumberUtility.isIntegerNumber(sender, 10);
        }

        private void UnitWeight_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9]+");
            e.Handled = re.IsMatch(e.Text);
        }

        private void UnitWeight_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            NumberUtility.isIntegerNumber(sender, 100);
        }
    }
}
