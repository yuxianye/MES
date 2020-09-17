using Solution.Desktop.Core;

namespace Solution.Desktop.ProductionProcessInfo.View
{
    /// <summary>
    /// EnterpriseInfoEditView.xaml 的交互逻辑
    /// </summary>
    public partial class ProductionProcessInfoEditView : UserControlBase
    {
        public ProductionProcessInfoEditView()
        {
            InitializeComponent();
            processCode.Focus();
        }
    }
}
