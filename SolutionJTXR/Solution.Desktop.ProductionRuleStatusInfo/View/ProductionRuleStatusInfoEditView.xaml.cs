using Solution.Desktop.Core;

namespace Solution.Desktop.ProductionRuleStatusInfo.View
{
    /// <summary>
    /// EnterpriseInfoEditView.xaml 的交互逻辑
    /// </summary>
    public partial class ProductionRuleStatusInfoEditView : UserControlBase
    {
        public ProductionRuleStatusInfoEditView()
        {
            InitializeComponent();
            productionRuleStatusCode.Focus();
        }
    }
}
