using Solution.Desktop.Core;
using Solution.Desktop.MatWareHouseInfo.Model;
using System.Text.RegularExpressions;

namespace Solution.Desktop.MatStorageModifyInfo.View
{
    /// <summary>
    /// MatStorageModifyInfoAddView.xaml 的交互逻辑
    /// </summary>
    public partial class MatStorageModifyInfoAddView : UserControlBase
    {
        public MatStorageModifyInfoAddView()
        {
            InitializeComponent();
            StorageModifyCode.Focus();
        }

        private void CurrentAmount_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9]+");
            e.Handled = re.IsMatch(e.Text);
        }

        private void CurrentAmount_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            NumberUtility.isIntegerNumber(sender, 500);
        }
    }
}
