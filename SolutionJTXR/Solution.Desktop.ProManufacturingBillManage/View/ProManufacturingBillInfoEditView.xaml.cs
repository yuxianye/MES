using Solution.Desktop.Core;

namespace Solution.Desktop.ProManufacturingBillManage.View
{
    /// <summary>
    /// EnterpriseInfoEditView.xaml 的交互逻辑
    /// </summary>
    public partial class ProManufacturingBillInfoEditView : UserControlBase
    {
        public ProManufacturingBillInfoEditView()
        {
            InitializeComponent();
            billName.Focus();
        }
    }
}
