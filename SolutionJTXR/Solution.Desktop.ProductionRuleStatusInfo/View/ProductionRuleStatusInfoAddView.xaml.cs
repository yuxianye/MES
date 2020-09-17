using Solution.Desktop.Core;

namespace Solution.Desktop.ProductionRuleStatusInfo.View
{
    /// <summary>
    /// EnterpriseInfoAddView.xaml 的交互逻辑
    /// </summary>
    public partial class ProductionRuleStatusInfoAddView : UserControlBase
    {
        public ProductionRuleStatusInfoAddView()
        {
            InitializeComponent();
            productionRuleStatusCode.Focus();
        }

        private void EnterpriseInfoList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            
        }
    }
}
