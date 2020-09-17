using Solution.Desktop.Core;
using Solution.Desktop.MatWareHouseInfo.Model;
using System.Text.RegularExpressions;

namespace Solution.Desktop.MatInventoryInfo.View
{
    /// <summary>
    /// MatInventoryItemInfoAddView.xaml 的交互逻辑
    /// </summary>
    public partial class MatInventoryItemInfoAddView : UserControlBase
    {
        public MatInventoryItemInfoAddView()
        {
            InitializeComponent();
            MaterialBatch_Id.Focus();
        }

        private void ActualAmount_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9]+");
            e.Handled = re.IsMatch(e.Text);
        }

        private void ActualAmount_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            NumberUtility.isIntegerNumber(sender, 500);
        }
    }
}
