using Solution.Desktop.Core;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Solution.Desktop.ProductionRuleManage.View
{
    /// <summary>
    /// EnterpriseInfoEditView.xaml 的交互逻辑
    /// </summary>
    public partial class ProductionRuleInfoEditView : UserControlBase
    {
        public ProductionRuleInfoEditView()
        {
            InitializeComponent();
            cmb_product.Focus();
        }
        private void tb_PreviewTextInput(object sender, TextCompositionEventArgs e)

        {

            Regex re = new Regex("[^0-9.]+");

            e.Handled = re.IsMatch(e.Text);

        }
    }
}
