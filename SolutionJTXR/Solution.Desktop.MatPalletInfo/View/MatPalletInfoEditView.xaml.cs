using Solution.Desktop.Core;
using Solution.Desktop.MatWareHouseInfo.Model;
using System.Text.RegularExpressions;

namespace Solution.Desktop.MatPalletInfo.View
{
    /// <summary>
    /// PalletInfoEditView.xaml 的交互逻辑
    /// </summary>
    public partial class MatPalletInfoEditView : UserControlBase
    {
        public MatPalletInfoEditView()
        {
            InitializeComponent();
            //firstIC.Focus();
        }

        private void PalletMaxWeight_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9.-]+");
            e.Handled = re.IsMatch(e.Text);
        }

        private void PalletMaxWeight_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            NumberUtility.isDecimalNumber(sender, 1000);
        }
    }
}
