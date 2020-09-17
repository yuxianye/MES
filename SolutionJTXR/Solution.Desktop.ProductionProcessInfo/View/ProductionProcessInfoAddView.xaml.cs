using Solution.Desktop.Core;

namespace Solution.Desktop.ProductionProcessInfo.View
{
    /// <summary>
    /// EnterpriseInfoAddView.xaml 的交互逻辑
    /// </summary>
    public partial class ProductionProcessInfoAddView : UserControlBase
    {
        public ProductionProcessInfoAddView()
        {
            InitializeComponent();
            cmb_enterprise.Focus();
        }

        private void EnterpriseInfoList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            
        }
    }
}
