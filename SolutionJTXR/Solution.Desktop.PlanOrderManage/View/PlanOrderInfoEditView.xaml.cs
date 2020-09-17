using Solution.Desktop.Core;

namespace Solution.Desktop.PlanOrderManage.View
{
    /// <summary>
    /// EnterpriseInfoEditView.xaml 的交互逻辑
    /// </summary>
    public partial class PlanOrderInfoEditView : UserControlBase
    {
        public PlanOrderInfoEditView()
        {
            InitializeComponent();
            orderCode.Focus();
        }
    }
}
