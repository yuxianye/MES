using Solution.Desktop.Core;

namespace Solution.Desktop.PlanOrderManage.View
{
    /// <summary>
    /// EnterpriseInfoAddView.xaml 的交互逻辑
    /// </summary>
    public partial class PlanOrderInfoAddView : UserControlBase
    {
        public PlanOrderInfoAddView()
        {
            InitializeComponent();
            orderCode.Focus();
        }

        private void EnterpriseInfoList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            
        }
    }
}
