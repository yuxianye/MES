using Solution.Desktop.Core;

namespace Solution.Desktop.ProManufacturingBillManage.View
{
    /// <summary>
    /// EnterpriseInfoAddView.xaml 的交互逻辑
    /// </summary>
    public partial class ProManufacturingBillInfoAddView : UserControlBase
    {
        public ProManufacturingBillInfoAddView()
        {
            InitializeComponent();
            cmb_billtype.Focus();
        }

        private void EnterpriseInfoList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            
        }
    }
}
