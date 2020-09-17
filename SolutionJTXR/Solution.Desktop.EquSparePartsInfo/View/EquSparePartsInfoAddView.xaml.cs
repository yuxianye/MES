using Solution.Desktop.Core;
using Solution.Desktop.EquSparePartsInfo.Model;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Solution.Desktop.EquSparePartsInfo.View
{
    /// <summary>
    /// EnterpriseInfoAddView.xaml 的交互逻辑
    /// </summary>
    public partial class EquSparePartsInfoAddView : UserControlBase
    {
        public EquSparePartsInfoAddView()
        {
            InitializeComponent();
            firstIC.Focus();
        }
        private void tb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            Regex re = new Regex("[^0-9.]+");

            e.Handled = re.IsMatch(e.Text);

        }

    }
}
